using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LighthouseExtends.UIComponent.TextMeshPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LighthouseExtends.TextTable.Editor
{
    public class TextTableEditorWindow : EditorWindow
    {
        const string MenuPath = "Lighthouse/TextTable/Open Editor";
        const string SettingsDefaultDirectory = "Assets/Settings/Lighthouse";
        const float LeftPanelMinWidth = 120f;
        const float LeftPanelDefaultWidth = 220f;
        const float KeyColumnWidth = 200f;
        const float LangColumnWidth = 160f;
        const float ResizeHandleWidth = 4f;

        // Settings
        TextTableEditorSettings settings;

        // Left panel
        float leftPanelWidth = LeftPanelDefaultWidth;
        bool isResizing;
        List<AssetEntry> sceneEntries = new();
        List<AssetEntry> prefabEntries = new();
        AssetEntry selectedEntry;
        Vector2 assetListScroll;

        // Right panel
        List<TextKeyRow> rows = new();
        List<string> languages = new();
        Dictionary<string, Dictionary<string, string>> tableData = new();
        // key -> language -> TSV base name that owns the key
        Dictionary<string, Dictionary<string, string>> keySourceMap = new();
        Vector2 tableScroll;
        bool isDirty;

        // Cached styles (initialized in OnGUI)
        GUIStyle normalKeyStyle;
        GUIStyle unregisteredKeyStyle;
        GUIStyle leftAlignedButtonStyle;

        // ─────────────────────────────────────────────────────────────────
        // Open
        // ─────────────────────────────────────────────────────────────────

        [MenuItem(MenuPath)]
        static void Open() => GetWindow<TextTableEditorWindow>("TextTable Editor");

        void OnEnable()
        {
            LoadOrCreateSettings();
            RefreshAssetList();
        }

        // ─────────────────────────────────────────────────────────────────
        // Settings
        // ─────────────────────────────────────────────────────────────────

        void LoadOrCreateSettings()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(TextTableEditorSettings)}");
            if (guids.Length > 0)
            {
                if (guids.Length > 1)
                {
                    Debug.LogWarning("[TextTable] Multiple TextTableEditorSettings found. Using the first one.");
                }

                settings = AssetDatabase.LoadAssetAtPath<TextTableEditorSettings>(
                    AssetDatabase.GUIDToAssetPath(guids[0]));
                return;
            }

            settings = CreateInstance<TextTableEditorSettings>();
            if (!Directory.Exists(SettingsDefaultDirectory))
            {
                Directory.CreateDirectory(SettingsDefaultDirectory);
            }

            AssetDatabase.CreateAsset(settings, $"{SettingsDefaultDirectory}/{nameof(TextTableEditorSettings)}.asset");
            AssetDatabase.SaveAssets();
        }

        // ─────────────────────────────────────────────────────────────────
        // Asset Discovery
        // ─────────────────────────────────────────────────────────────────

        void RefreshAssetList()
        {
            sceneEntries = AssetDatabase.FindAssets("t:SceneAsset")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(p => p.StartsWith("Assets/"))
                .Select(p => new AssetEntry(p, Path.GetFileNameWithoutExtension(p),
                    $"Scene{Path.GetFileNameWithoutExtension(p)}", isScene: true))
                .ToList();

            var prefabPaths = AssetDatabase.FindAssets("t:Prefab")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(p => p.StartsWith("Assets/"))
                .ToList();

            var baseNames = ComputePrefabBaseNames(prefabPaths);
            prefabEntries = prefabPaths
                .Select((p, i) => new AssetEntry(p, Path.GetFileNameWithoutExtension(p), baseNames[i], isScene: false))
                .ToList();
        }

        static List<string> ComputePrefabBaseNames(List<string> paths)
        {
            var depth = 1;
            var names = paths.Select(p => GetParentDirSuffix(p, depth)).ToList();

            for (var iter = 0; iter < 10 && HasDuplicates(names); iter++)
            {
                depth++;
                var duplicates = names.GroupBy(n => n)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToHashSet();

                for (var i = 0; i < names.Count; i++)
                {
                    if (duplicates.Contains(names[i]))
                    {
                        names[i] = GetParentDirSuffix(paths[i], depth);
                    }
                }
            }

            return names.Select(n => $"Prefab{n}").ToList();
        }

        static string GetParentDirSuffix(string assetPath, int depth)
        {
            var parts = assetPath.Replace('\\', '/').Split('/');
            var taken = new List<string>();
            for (var d = 1; d <= depth && d < parts.Length - 1; d++)
            {
                taken.Insert(0, parts[parts.Length - 1 - d]);
            }

            return string.Concat(taken);
        }

        static bool HasDuplicates(List<string> list) => list.Count != list.Distinct().Count();

        // ─────────────────────────────────────────────────────────────────
        // Data Loading
        // ─────────────────────────────────────────────────────────────────

        void SelectAsset(AssetEntry entry)
        {
            if (isDirty && !EditorUtility.DisplayDialog(
                    "Unsaved Changes",
                    "Discard unsaved changes and switch asset?",
                    "Discard", "Cancel"))
            {
                return;
            }

            selectedEntry = entry;
            LoadTableData();
            Repaint();
        }

        void LoadTableData()
        {
            rows = new List<TextKeyRow>();
            tableData = new Dictionary<string, Dictionary<string, string>>();
            keySourceMap = new Dictionary<string, Dictionary<string, string>>();
            isDirty = false;

            if (selectedEntry == null || settings == null)
            {
                return;
            }

            var folder = ToAbsolutePath(settings.TextTableFolderPath);
            languages = DetectLanguages(folder);
            var (allData, loadedSourceMap) = LoadAllTsvData(folder);
            keySourceMap = loadedSourceMap;

            var components = FindComponents(selectedEntry);

            var grouped = new Dictionary<string, List<string>>();
            foreach (var (comp, path) in components)
            {
                var key = ReadTextKey(comp);
                if (!grouped.ContainsKey(key))
                {
                    grouped[key] = new List<string>();
                }

                grouped[key].Add(path);
            }

            foreach (var (key, paths) in grouped)
            {
                rows.Add(new TextKeyRow(key, paths));
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                tableData[key] = new Dictionary<string, string>();
                foreach (var lang in languages)
                {
                    tableData[key][lang] = allData.TryGetValue(key, out var langMap) &&
                                           langMap.TryGetValue(lang, out var text)
                        ? text
                        : string.Empty;
                }
            }
        }

        static List<(LHTextMeshPro comp, string path)> FindComponents(AssetEntry entry)
        {
            LHTextMeshPro[] comps;
            if (entry.isScene)
            {
                EditorSceneManager.OpenScene(entry.assetPath, OpenSceneMode.Single);
                comps = SceneManager.GetActiveScene()
                    .GetRootGameObjects()
                    .SelectMany(go => go.GetComponentsInChildren<LHTextMeshPro>(true))
                    .ToArray();
            }
            else
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(entry.assetPath);
                comps = prefab != null
                    ? prefab.GetComponentsInChildren<LHTextMeshPro>(true)
                    : Array.Empty<LHTextMeshPro>();
            }

            return comps.Select(c => (c, GetHierarchyPath(c.gameObject))).ToList();
        }

        // ─────────────────────────────────────────────────────────────────
        // TSV I/O
        // ─────────────────────────────────────────────────────────────────

        static List<string> DetectLanguages(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return new List<string>();
            }

            return Directory.GetFiles(folder, "*.tsv")
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .Where(n => n.Contains('.'))
                .Select(n => n.Substring(n.LastIndexOf('.') + 1))
                .Distinct()
                .OrderBy(l => l)
                .ToList();
        }

        static (Dictionary<string, Dictionary<string, string>> data,
            Dictionary<string, Dictionary<string, string>> sourceMap)
            LoadAllTsvData(string folder)
        {
            var data = new Dictionary<string, Dictionary<string, string>>();
            var sourceMap = new Dictionary<string, Dictionary<string, string>>();

            if (!Directory.Exists(folder))
            {
                return (data, sourceMap);
            }

            foreach (var file in Directory.GetFiles(folder, "*.tsv"))
            {
                var nameNoExt = Path.GetFileNameWithoutExtension(file);
                var dot = nameNoExt.LastIndexOf('.');
                if (dot < 0)
                {
                    continue;
                }

                var lang = nameNoExt.Substring(dot + 1);
                var baseName = nameNoExt.Substring(0, dot);

                foreach (var line in File.ReadAllLines(file).Skip(1))
                {
                    var trimmed = line.TrimEnd('\r');
                    if (string.IsNullOrEmpty(trimmed))
                    {
                        continue;
                    }

                    var tab = trimmed.IndexOf('\t');
                    if (tab < 0)
                    {
                        continue;
                    }

                    var key = trimmed.Substring(0, tab);
                    var text = trimmed.Substring(tab + 1);

                    if (!data.ContainsKey(key))
                    {
                        data[key] = new Dictionary<string, string>();
                    }

                    data[key][lang] = text;

                    if (!sourceMap.ContainsKey(key))
                    {
                        sourceMap[key] = new Dictionary<string, string>();
                    }

                    sourceMap[key][lang] = baseName;
                }
            }

            return (data, sourceMap);
        }

        void Save()
        {
            if (selectedEntry == null || settings == null)
            {
                return;
            }

            var folder = ToAbsolutePath(settings.TextTableFolderPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (var lang in languages)
            {
                var filePath = Path.Combine(folder, $"{selectedEntry.tsvBaseName}.{lang}.tsv");

                var existing = new Dictionary<string, string>();
                if (File.Exists(filePath))
                {
                    foreach (var line in File.ReadAllLines(filePath).Skip(1))
                    {
                        var trimmed = line.TrimEnd('\r');
                        if (string.IsNullOrEmpty(trimmed))
                        {
                            continue;
                        }

                        var tab = trimmed.IndexOf('\t');
                        if (tab < 0)
                        {
                            continue;
                        }

                        existing[trimmed.Substring(0, tab)] = trimmed.Substring(tab + 1);
                    }
                }

                foreach (var (key, langMap) in tableData)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }

                    // Skip keys owned by a different TSV file
                    if (keySourceMap.TryGetValue(key, out var sm) &&
                        sm.TryGetValue(lang, out var src) &&
                        src != selectedEntry.tsvBaseName)
                    {
                        continue;
                    }

                    if (langMap.TryGetValue(lang, out var text))
                    {
                        existing[key] = text;
                    }
                }

                var sb = new StringBuilder("key\ttext\n");
                foreach (var (key, text) in existing)
                {
                    sb.Append($"{key}\t{text}\n");
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            }

            AssetDatabase.Refresh();
            isDirty = false;
        }

        // ─────────────────────────────────────────────────────────────────
        // GUI
        // ─────────────────────────────────────────────────────────────────

        void OnGUI()
        {
            EnsureStyles();
            DrawToolbar();

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawAssetList();
                DrawResizeHandle();
                DrawTable();
            }

            HandleResize();
        }

        void DrawResizeHandle()
        {
            var handleRect = GUILayoutUtility.GetRect(ResizeHandleWidth, 0, GUILayout.Width(ResizeHandleWidth), GUILayout.ExpandHeight(true));
            EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.ResizeHorizontal);
            EditorGUI.DrawRect(handleRect, new Color(0f, 0f, 0f, 0.2f));

            if (Event.current.type == EventType.MouseDown && handleRect.Contains(Event.current.mousePosition))
            {
                isResizing = true;
                Event.current.Use();
            }
        }

        void HandleResize()
        {
            if (!isResizing)
            {
                return;
            }

            if (Event.current.type == EventType.MouseDrag)
            {
                leftPanelWidth = Mathf.Max(LeftPanelMinWidth, Event.current.mousePosition.x);
                Repaint();
                Event.current.Use();
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                isResizing = false;
                Event.current.Use();
            }
        }

        void EnsureStyles()
        {
            if (normalKeyStyle != null)
            {
                return;
            }

            normalKeyStyle = new GUIStyle(EditorStyles.boldLabel);
            unregisteredKeyStyle = new GUIStyle(EditorStyles.boldLabel);
            unregisteredKeyStyle.normal.textColor = new Color(0.9f, 0.5f, 0f);
            leftAlignedButtonStyle = new GUIStyle(EditorStyles.miniButton);
            leftAlignedButtonStyle.alignment = TextAnchor.MiddleLeft;
        }

        void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                var next = (TextTableEditorSettings)EditorGUILayout.ObjectField(
                    settings, typeof(TextTableEditorSettings), false, GUILayout.Width(320));
                if (next != settings)
                {
                    settings = next;
                    selectedEntry = null;
                    rows = new List<TextKeyRow>();
                    tableData = new Dictionary<string, Dictionary<string, string>>();
                    RefreshAssetList();
                }

                GUILayout.FlexibleSpace();

                using (new EditorGUI.DisabledScope(!isDirty))
                {
                    if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(60)))
                    {
                        Save();
                    }
                }
            }
        }

        void DrawAssetList()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(leftPanelWidth), GUILayout.ExpandHeight(true)))
            {
                assetListScroll = EditorGUILayout.BeginScrollView(assetListScroll);

                DrawAssetGroup("Scenes", sceneEntries);
                GUILayout.Space(6);
                DrawAssetGroup("Prefabs", prefabEntries);

                EditorGUILayout.EndScrollView();
            }
        }

        void DrawAssetGroup(string label, List<AssetEntry> entries)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            var groups = entries
                .GroupBy(e => Path.GetDirectoryName(e.assetPath).Replace('\\', '/'))
                .OrderBy(g => g.Key);

            foreach (var group in groups)
            {
                var dirLabel = group.Key;
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(dirLabel, EditorStyles.miniLabel, GUILayout.Width(leftPanelWidth - 16));
                EditorGUI.indentLevel--;

                foreach (var entry in group)
                {
                    DrawAssetButton(entry);
                }
            }
        }

        void DrawAssetButton(AssetEntry entry)
        {
            var isSelected = selectedEntry == entry;
            using (new BackgroundColorScope(isSelected ? new Color(0.4f, 0.6f, 1f) : Color.white))
            {
                if (GUILayout.Button(entry.displayName, leftAlignedButtonStyle))
                {
                    SelectAsset(entry);
                    var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(entry.assetPath);
                    if (asset != null)
                    {
                        EditorGUIUtility.PingObject(asset);
                        if (!entry.isScene)
                        {
                            Selection.activeObject = asset;
                        }
                    }
                }
            }
        }

        void DrawTable()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (selectedEntry == null)
                {
                    EditorGUILayout.HelpBox("Select a Scene or Prefab from the list.", MessageType.Info);
                    return;
                }

                if (languages.Count == 0)
                {
                    EditorGUILayout.HelpBox(
                        $"No .tsv files found in: {settings?.TextTableFolderPath}", MessageType.Warning);
                }

                tableScroll = EditorGUILayout.BeginScrollView(tableScroll);
                DrawTableHeader();
                foreach (var row in rows)
                {
                    DrawTableRow(row);
                }

                EditorGUILayout.EndScrollView();
            }
        }

        void DrawTableHeader()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUILayout.LabelField("TextKey / Components", EditorStyles.toolbarButton,
                    GUILayout.Width(KeyColumnWidth));
                foreach (var lang in languages)
                {
                    EditorGUILayout.LabelField(lang, EditorStyles.toolbarButton,
                        GUILayout.Width(LangColumnWidth));
                }
            }
        }

        void DrawTableRow(TextKeyRow row)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                // Key column
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(KeyColumnWidth)))
                {
                    var isUnregistered = !string.IsNullOrEmpty(row.textKey) &&
                                         tableData.TryGetValue(row.textKey, out var ld) &&
                                         ld.Values.All(string.IsNullOrEmpty);

                    var keyStyle = isUnregistered ? unregisteredKeyStyle : normalKeyStyle;
                    EditorGUILayout.LabelField(
                        string.IsNullOrEmpty(row.textKey) ? "(no key)" : row.textKey,
                        keyStyle, GUILayout.Width(KeyColumnWidth));

                    foreach (var path in row.componentPaths)
                    {
                        EditorGUILayout.LabelField(path, EditorStyles.miniLabel,
                            GUILayout.Width(KeyColumnWidth));
                    }
                }

                // Language columns
                if (!string.IsNullOrEmpty(row.textKey) &&
                    tableData.TryGetValue(row.textKey, out var langData))
                {
                    foreach (var lang in languages)
                    {
                        var current = langData.TryGetValue(lang, out var t) ? t : string.Empty;
                        var isReadOnly = keySourceMap.TryGetValue(row.textKey, out var sm) &&
                                         sm.TryGetValue(lang, out var src) &&
                                         src != selectedEntry.tsvBaseName;

                        using (new EditorGUI.DisabledScope(isReadOnly))
                        {
                            var next = EditorGUILayout.TextField(current, GUILayout.Width(LangColumnWidth));
                            if (!isReadOnly && next != current)
                            {
                                langData[lang] = next;
                                isDirty = true;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var _ in languages)
                    {
                        GUILayout.Space(LangColumnWidth);
                    }
                }
            }

            EditorGUILayout.Space(1);
        }

        // ─────────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────────

        static string ReadTextKey(LHTextMeshPro comp)
        {
            var so = new SerializedObject(comp);
            return so.FindProperty("textKey")?.stringValue ?? string.Empty;
        }

        static string GetHierarchyPath(GameObject go)
        {
            var parts = new List<string>();
            var t = go.transform;
            while (t != null)
            {
                parts.Insert(0, t.name);
                t = t.parent;
            }

            return string.Join("/", parts);
        }

        static string ToAbsolutePath(string unityPath)
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", unityPath))
                .Replace('\\', '/');
        }

        // ─────────────────────────────────────────────────────────────────
        // Helper scope
        // ─────────────────────────────────────────────────────────────────

        struct BackgroundColorScope : IDisposable
        {
            readonly Color previous;

            public BackgroundColorScope(Color color)
            {
                previous = GUI.backgroundColor;
                GUI.backgroundColor = color;
            }

            public void Dispose() => GUI.backgroundColor = previous;
        }

        // ─────────────────────────────────────────────────────────────────
        // Data types
        // ─────────────────────────────────────────────────────────────────

        class AssetEntry
        {
            public readonly string assetPath;
            public readonly string displayName;
            public readonly string tsvBaseName;
            public readonly bool isScene;

            public AssetEntry(string assetPath, string displayName, string tsvBaseName, bool isScene)
            {
                this.assetPath = assetPath;
                this.displayName = displayName;
                this.tsvBaseName = tsvBaseName;
                this.isScene = isScene;
            }
        }

        class TextKeyRow
        {
            public readonly string textKey;
            public readonly List<string> componentPaths;

            public TextKeyRow(string textKey, List<string> componentPaths)
            {
                this.textKey = textKey;
                this.componentPaths = componentPaths;
            }
        }
    }
}
