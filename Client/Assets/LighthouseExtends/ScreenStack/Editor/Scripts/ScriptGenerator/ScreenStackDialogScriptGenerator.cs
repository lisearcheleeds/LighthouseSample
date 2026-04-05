using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LighthouseExtends.ScreenStack.Editor.ScriptableObject;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace LighthouseExtends.ScreenStack.Editor.ScriptGenerator
{
    public class ScreenStackDialogBaseClassInfo
    {
        public string TypeName { get; set; }
        public string Namespace { get; set; }

        public string DropdownLabel => $"{TypeName}  ({Namespace})";
    }

    public static class ScreenStackDialogScriptGenerator
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

            var loadedNames =
                new HashSet<string>(AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name));

            return compiledNames.Any(name => !loadedNames.Contains(name));
        }

        public static ScreenStackDialogBaseClassInfo[] CollectBaseClasses()
        {
            var rootType = typeof(ScreenStackBase);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => t.IsAbstract && !t.IsInterface)
                .Where(t => t == rootType || InheritsFrom(t, rootType))
                .Select(t => new ScreenStackDialogBaseClassInfo
                {
                    TypeName = t.Name,
                    Namespace = t.Namespace ?? string.Empty
                })
                .OrderBy(b => b.Namespace)
                .ThenBy(b => b.TypeName)
                .ToArray();
        }

        public static void GenerateFiles(string dialogName, ScreenStackDialogBaseClassInfo baseClass,
            ScreenStackGenerateSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("[ScreenStackDialogScriptGenerator] ScreenStackGenerateSettings is null.");
                return;
            }

            var outputRoot = settings.ScreenStackDialogScriptOutputDirectory;
            if (string.IsNullOrEmpty(outputRoot))
            {
                Debug.LogError(
                    "[ScreenStackDialogScriptGenerator] Output directory is not configured in ScreenStackGenerateSettings.");
                return;
            }

            var namespaceName = string.IsNullOrEmpty(settings.ScreenStackDialogScriptNamespace)
                ? dialogName
                : $"{settings.ScreenStackDialogScriptNamespace}.{dialogName}";

            var replacements = new Dictionary<string, string>
            {
                { "{{NAMESPACE}}", namespaceName },
                { "{{DIALOG_NAME}}", dialogName },
                { "{{DIALOG_NAME_CAMEL}}", ToCamelCase(dialogName) },
                { "{{BASE_CLASS}}", baseClass.TypeName },
                { "{{BASE_CLASS_NAMESPACE}}", baseClass.Namespace }
            };

            var outputAssetDir = $"{outputRoot}/{dialogName}";
            var outputFsDir = AssetPathToFsPath(outputAssetDir);

            if (!Directory.Exists(outputFsDir))
            {
                Directory.CreateDirectory(outputFsDir);
            }

            var files = new[]
            {
                ($"{outputAssetDir}/{dialogName}Dialog.cs", LoadTemplate(settings.ScreenStackDialogDialogTemplate)),
                ($"{outputAssetDir}/{dialogName}Data.cs", LoadTemplate(settings.ScreenStackDialogDataTemplate)),
                ($"{outputAssetDir}/{dialogName}Presenter.cs",
                    LoadTemplate(settings.ScreenStackDialogPresenterTemplate)),
                ($"{outputAssetDir}/{dialogName}View.cs", LoadTemplate(settings.ScreenStackDialogViewTemplate))
            };

            foreach (var (assetPath, templateContent) in files)
            {
                WriteFromTemplate(templateContent, replacements, AssetPathToFsPath(assetPath));
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
            }

            Debug.Log(
                $"[ScreenStackDialogScriptGenerator] Generated dialog scripts for '{dialogName}' at {outputAssetDir}");
        }

        public static string GetTemplateValidationError(ScreenStackGenerateSettings settings)
        {
            if (settings.ScreenStackDialogDialogTemplate == null
                || settings.ScreenStackDialogDataTemplate == null
                || settings.ScreenStackDialogPresenterTemplate == null
                || settings.ScreenStackDialogViewTemplate == null)
            {
                return
                    "One or more ScreenStack Dialog script templates are not configured in ScreenStackGenerateSettings.\n"
                    + "Please assign all templates under \"ScreenStack Dialog Script Templates\".";
            }

            return null;
        }

        static string LoadTemplate(TextAsset template)
        {
            if (template != null)
            {
                return template.text;
            }

            Debug.LogError(
                "[ScreenStackDialogScriptGenerator] A required template is not assigned in ScreenStackGenerateSettings.");
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

                Debug.LogWarning($"[ScreenStackDialogScriptGenerator] Overwriting: {Path.GetFileName(fsPath)}");
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

        static bool InheritsFrom(Type type, Type baseType)
        {
            var current = type.BaseType;
            while (current != null && current != typeof(object))
            {
                if (current == baseType)
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
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
