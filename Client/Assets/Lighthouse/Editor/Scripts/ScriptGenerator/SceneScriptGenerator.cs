using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lighthouse.Editor.ScriptableObject;
using Lighthouse.Scene.SceneBase;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace Lighthouse.Editor.ScriptGenerator
{
    public enum SceneType
    {
        MainScene,
        ModuleScene
    }

    public class BaseClassInfo
    {
        public string TypeName { get; set; }
        public string Namespace { get; set; }
        public bool IsGeneric { get; set; }

        public string DropdownLabel
        {
            get
            {
                return IsGeneric ? $"{TypeName}<T>  ({Namespace})" : $"{TypeName}  ({Namespace})";
            }
        }

        public string GetBaseClassExpression(string sceneFileName, string sceneName)
        {
            return IsGeneric ? $"{TypeName}<{sceneFileName}.{sceneName}TransitionData>" : TypeName;
        }
    }

    public static class SceneScriptGenerator
    {
        public static bool IsCompiling => EditorApplication.isCompiling;

        public static bool HasCompileErrors()
        {
            if (EditorApplication.isCompiling)
            {
                return false;
            }

            var compiledNames = new HashSet<string>(
                CompilationPipeline.GetAssemblies(AssembliesType.Editor)
                    .Concat(CompilationPipeline.GetAssemblies(AssembliesType.Player))
                    .Select(a => a.name));

            var loadedNames = new HashSet<string>(AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name));

            return compiledNames.Any(name => !loadedNames.Contains(name));
        }

        public static BaseClassInfo[] CollectBaseClasses(SceneType sceneType)
        {
            var rootType = sceneType == SceneType.MainScene
                ? typeof(MainSceneBase)
                : typeof(ModuleSceneBase);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => t.IsAbstract && !t.IsInterface)
                .Where(t => t == rootType || InheritsFrom(t, rootType))
                .Select(t => new BaseClassInfo
                {
                    TypeName = CleanTypeName(t.Name),
                    Namespace = t.Namespace ?? string.Empty,
                    IsGeneric = t.IsGenericTypeDefinition
                })
                .OrderBy(b => b.Namespace)
                .ThenBy(b => b.TypeName)
                .ToArray();
        }

        public static void GenerateFiles(string sceneName, SceneType sceneType, BaseClassInfo baseClass, GenerateSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("[SceneScriptGenerator] GenerateSettings is null.");
                return;
            }

            var outputRoot = settings.SceneScriptOutputDirectory;
            if (string.IsNullOrEmpty(outputRoot))
            {
                Debug.LogError("[SceneScriptGenerator] Output directory is not configured in GenerateSettings.");
                return;
            }

            var sceneTypeName = sceneType == SceneType.MainScene ? "MainScene" : "ModuleScene";
            var sceneFileName = sceneType == SceneType.MainScene ? $"{sceneName}Scene" : $"{sceneName}ModuleScene";

            var replacements = new Dictionary<string, string>
            {
                { "{{NAMESPACE}}", $"{settings.ProductNameSpace}.View.Scene.{sceneTypeName}.{sceneName}" },
                { "{{SCENE_NAME}}", sceneName },
                { "{{SCENE_NAME_CAMEL}}", ToCamelCase(sceneName) },
                { "{{SCENE_FILE_NAME}}", sceneFileName },
                { "{{SCENE_FILE_NAME_CAMEL}}", ToCamelCase(sceneFileName) },
                { "{{BASE_CLASS}}", baseClass.GetBaseClassExpression(sceneFileName, sceneName) },
                { "{{BASE_CLASS_NAMESPACE}}", baseClass.Namespace },
                { "{{SCENE_ID_TYPE}}", sceneType == SceneType.MainScene ? "MainSceneId" : "ModuleSceneId" },
                {
                    "{{SCENE_ID_CLASS}}",
                    sceneType == SceneType.MainScene
                        ? settings.GeneratedMainSceneIdClassName
                        : settings.GeneratedModuleSceneIdClassName
                },
                { "{{GENERATED_SCENE_ID_NAMESPACE}}", settings.GeneratedSceneIdNamespace }
            };

            var outputAssetDir = $"{outputRoot}/{sceneTypeName}/{sceneName}";
            var outputFsDir = AssetPathToFsPath(outputAssetDir);

            if (!Directory.Exists(outputFsDir))
            {
                Directory.CreateDirectory(outputFsDir);
            }

            var lifetimeScopeTemplate = sceneType == SceneType.MainScene
                ? settings.LifetimeScopeMainSceneTemplate
                : settings.LifetimeScopeModuleSceneTemplate;
            var sceneTemplate = sceneType == SceneType.MainScene
                ? settings.SceneMainSceneTemplate
                : settings.SceneModuleSceneTemplate;

            var files = new[]
            {
                ($"{outputAssetDir}/{sceneName}LifetimeScope.cs", LoadTemplate(lifetimeScopeTemplate)),
                ($"{outputAssetDir}/{sceneFileName}.cs", LoadTemplate(sceneTemplate)),
                ($"{outputAssetDir}/I{sceneName}Presenter.cs", LoadTemplate(settings.IPresenterTemplate)),
                ($"{outputAssetDir}/{sceneName}Presenter.cs", LoadTemplate(settings.PresenterTemplate)),
                ($"{outputAssetDir}/I{sceneName}View.cs", LoadTemplate(settings.IViewTemplate)),
                ($"{outputAssetDir}/{sceneName}View.cs", LoadTemplate(settings.ViewTemplate)),
            };

            foreach (var (assetPath, templateContent) in files)
            {
                WriteFromTemplate(templateContent, replacements, AssetPathToFsPath(assetPath));
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
            }

            Debug.Log($"[SceneScriptGenerator] Generated scene scripts for '{sceneName}' at {outputAssetDir}");
        }

        public static string GetTemplateValidationError(GenerateSettings settings)
        {
            if (settings.LifetimeScopeMainSceneTemplate == null
                || settings.LifetimeScopeModuleSceneTemplate == null
                || settings.SceneMainSceneTemplate == null
                || settings.SceneModuleSceneTemplate == null
                || settings.IPresenterTemplate == null
                || settings.PresenterTemplate == null
                || settings.IViewTemplate == null
                || settings.ViewTemplate == null)
            {
                return "One or more scene script templates are not configured in GenerateSettings.\n"
                       + "Please assign all templates under \"Scene Script Templates\".";
            }

            return null;
        }

        static string LoadTemplate(TextAsset template)
        {
            if (template != null)
            {
                return template.text;
            }

            Debug.LogError("[SceneScriptGenerator] A required template is not assigned in GenerateSettings.");
            return string.Empty;
        }

        static void WriteFromTemplate(string template, Dictionary<string, string> replacements, string fsPath)
        {
            if (string.IsNullOrEmpty(template))
            {
                return;
            }

            var content = replacements.Aggregate(template, (current, pair) => current.Replace(pair.Key, pair.Value));

            if (File.Exists(fsPath))
            {
                var existing = File.ReadAllText(fsPath, Encoding.UTF8);
                if (existing == content)
                {
                    return;
                }

                Debug.LogWarning($"[SceneScriptGenerator] Overwriting: {Path.GetFileName(fsPath)}");
            }

            File.WriteAllText(fsPath, content, new UTF8Encoding(false));
        }

        static string AssetPathToFsPath(string assetPath)
        {
            if (assetPath.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
            {
                assetPath = assetPath.Substring("Assets/".Length);
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, assetPath));
        }

        // Traverse BaseType chain handling open generic types
        static bool InheritsFrom(Type type, Type baseType)
        {
            var current = type.BaseType;
            while (current != null && current != typeof(object))
            {
                var normalized = current.IsGenericType
                    ? current.GetGenericTypeDefinition()
                    : current;
                if (normalized == baseType)
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
        }

        // Remove generic arity suffix (e.g. "MainSceneBase`1" -> "MainSceneBase")
        static string CleanTypeName(string name)
        {
            var backtick = name.IndexOf('`');
            return backtick >= 0 ? name.Substring(0, backtick) : name;
        }

        static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }
    }
}
