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
        const float ResizeHandleWidth = 4f;
        const float ColHandleWidth = 6f;
        const float MinColWidth = 60f;

        // Settings
        TextTableEditorSettings settings;

        // Left panel
        float leftPanelWidth = LeftPanelDefaultWidth;
        bool isResizingPanel;

        List<AssetEntry> sceneEntries = new();
        List<AssetEntry> prefabEntries = new();
        AssetEntry selectedEntry;
        Vector2 assetListScroll;

        // Toolbar state
        Rect addLanguageButtonRect;

        // Right panel
        List<ComponentRow> rows = new();
        List<string> languages = new();
        Dictionary<string, Dictionary<string, string>> allTsvData = new();
        Dictionary<string, Dictionary<string, string>> keySourceMap = new();
        Vector2 tableScroll;
        bool isDirty;

        // Column widths (resizable)
        float pathColWidth = 200f;
        float placeholderColWidth = 160f;
        float keyColWidth = 180f;
        List<float> langColWidths = new();

        // Column resize state
        readonly List<Rect> colHandleRects = new();
        int resizingColIndex = -1;
        float resizeStartMouseX;
        float resizeStartColWidth;

        // Cached styles
        GUIStyle leftAlignedButtonStyle;
        GUIStyle dirtyTextFieldStyle;

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
        // Column width helpers
        // ─────────────────────────────────────────────────────────────────

        // Column index: 0=path, 1=placeholder, 2=key, 3+=lang
        float GetColWidth(int index)
        {
            if (index == 0) { return pathColWidth; }
            if (index == 1) { return placeholderColWidth; }
            if (index == 2) { return keyColWidth; }
            var li = index - 3;
            return li < langColWidths.Count ? langColWidths[li] : 160f;
        }

        void SetColWidth(int index, float width)
        {
            var w = Mathf.Max(MinColWidth, width);
            if (index == 0) { pathColWidth = w; return; }
            if (index == 1) { placeholderColWidth = w; return; }
            if (index == 2) { keyColWidth = w; return; }
            var li = index - 3;
            if (li < langColWidths.Count) { langColWidths[li] = w; }
        }

        void SyncLangColWidths()
        {
            while (langColWidths.Count < languages.Count) { langColWidths.Add(160f); }
            while (langColWidths.Count > languages.Count) { langColWidths.RemoveAt(langColWidths.Count - 1); }
        }

        // ─────────────────────────────────────────────────────────────────
        // Data Loading
        // ─────────────────────────────────────────────────────────────────

        void SelectAsset(AssetEntry entry)
        {
            if (isDirty)
            {
                // 0 = Save, 1 = Cancel, 2 = Discard
                var choice = EditorUtility.DisplayDialogComplex(
                    "Unsaved Changes",
                    "There are unsaved changes.",
                    "Save",
                    "Cancel",
                    "Discard");

                if (choice == 1)
                {
                    return;
                }

                if (choice == 0)
                {
                    Save();
                }
            }

            selectedEntry = entry;
            LoadTableData();
            Repaint();
        }

        void LoadTableData()
        {
            rows = new List<ComponentRow>();
            allTsvData = new Dictionary<string, Dictionary<string, string>>();
            keySourceMap = new Dictionary<string, Dictionary<string, string>>();
            isDirty = false;

            if (selectedEntry == null || settings == null)
            {
                return;
            }

            var folder = ToAbsolutePath(settings.TextTableFolderPath);
            languages = DetectLanguages(folder);
            SyncLangColWidths();

            var (loaded, loadedSourceMap) = LoadAllTsvData(folder);
            allTsvData = loaded;
            keySourceMap = loadedSourceMap;

            foreach (var (comp, path) in FindComponents(selectedEntry))
            {
                var key = ReadTextKey(comp);
                var langData = new Dictionary<string, string>();
                foreach (var lang in languages)
                {
                    langData[lang] = allTsvData.TryGetValue(key, out var langMap) &&
                                     langMap.TryGetValue(lang, out var text)
                        ? text
                        : string.Empty;
                }

                var placeholder = ReadPlaceholderText(comp);
                rows.Add(new ComponentRow(path, key, placeholder, comp, langData));
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

            // Save TextKey changes to Scene / Prefab
            if (rows.Any(r => r.IsTextKeyDirty))
            {
                foreach (var row in rows)
                {
                    if (!row.IsTextKeyDirty)
                    {
                        continue;
                    }

                    var so = new SerializedObject(row.component);
                    so.FindProperty("textKey").stringValue = row.textKey;
                    so.ApplyModifiedProperties();
                }

                if (selectedEntry.isScene)
                {
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                }
                else
                {
                    AssetDatabase.SaveAssets();
                }
            }

            // Save TSV files
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

                var writtenKeys = new HashSet<string>();
                foreach (var row in rows)
                {
                    var key = row.textKey;
                    if (string.IsNullOrEmpty(key) || !writtenKeys.Add(key))
                    {
                        continue;
                    }

                    if (keySourceMap.TryGetValue(key, out var sm) &&
                        sm.TryGetValue(lang, out var src) &&
                        src != selectedEntry.tsvBaseName)
                    {
                        continue;
                    }

                    if (row.langData.TryGetValue(lang, out var text))
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

            foreach (var row in rows)
            {
                row.MarkSaved();
            }

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
                DrawPanelResizeHandle();
                DrawTable();
            }

            HandlePanelResize();
            HandleColumnResize();
        }

        void DrawPanelResizeHandle()
        {
            var handleRect = GUILayoutUtility.GetRect(ResizeHandleWidth, 0,
                GUILayout.Width(ResizeHandleWidth), GUILayout.ExpandHeight(true));
            EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.ResizeHorizontal);
            EditorGUI.DrawRect(handleRect, new Color(0f, 0f, 0f, 0.2f));

            if (Event.current.type == EventType.MouseDown && handleRect.Contains(Event.current.mousePosition))
            {
                isResizingPanel = true;
                Event.current.Use();
            }
        }

        void HandlePanelResize()
        {
            if (!isResizingPanel)
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
                isResizingPanel = false;
                Event.current.Use();
            }
        }

        void HandleColumnResize()
        {
            var e = Event.current;

            // Always register cursor rects from last Repaint
            for (var i = 0; i < colHandleRects.Count; i++)
            {
                EditorGUIUtility.AddCursorRect(colHandleRects[i], MouseCursor.ResizeHorizontal);
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                for (var i = 0; i < colHandleRects.Count; i++)
                {
                    if (!colHandleRects[i].Contains(e.mousePosition))
                    {
                        continue;
                    }

                    resizingColIndex = i;
                    resizeStartMouseX = e.mousePosition.x;
                    resizeStartColWidth = GetColWidth(i);
                    e.Use();
                    break;
                }
            }
            else if (e.type == EventType.MouseDrag && resizingColIndex >= 0)
            {
                SetColWidth(resizingColIndex, resizeStartColWidth + (e.mousePosition.x - resizeStartMouseX));
                Repaint();
                e.Use();
            }
            else if (e.type == EventType.MouseUp && resizingColIndex >= 0)
            {
                resizingColIndex = -1;
                e.Use();
            }
        }

        void EnsureStyles()
        {
            if (leftAlignedButtonStyle == null)
            {
                leftAlignedButtonStyle = new GUIStyle(EditorStyles.miniButton)
                {
                    alignment = TextAnchor.MiddleLeft
                };
            }

            if (dirtyTextFieldStyle == null)
            {
                dirtyTextFieldStyle = new GUIStyle(EditorStyles.textField);
                dirtyTextFieldStyle.normal.textColor = new Color(1f, 0.85f, 0f);
                dirtyTextFieldStyle.focused.textColor = new Color(1f, 0.85f, 0f);
            }
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
                    rows = new List<ComponentRow>();
                    allTsvData = new Dictionary<string, Dictionary<string, string>>();
                    RefreshAssetList();
                }

                using (new EditorGUI.DisabledScope(settings == null))
                {
                    if (GUILayout.Button("Check Duplicates", EditorStyles.toolbarButton, GUILayout.Width(110)))
                    {
                        CheckDuplicateKeys();
                    }
                }

                GUILayout.FlexibleSpace();

                using (new EditorGUI.DisabledScope(settings == null))
                {
                    if (GUILayout.Button("Add Language", EditorStyles.toolbarButton, GUILayout.Width(90)))
                    {
                        var rect = addLanguageButtonRect;
                        EditorApplication.delayCall += () => PopupWindow.Show(rect, new AddLanguagePopup(AddLanguage));
                    }

                    if (Event.current.type == EventType.Repaint)
                    {
                        addLanguageButtonRect = GUILayoutUtility.GetLastRect();
                    }
                }

                using (new EditorGUI.DisabledScope(selectedEntry == null))
                {
                    if (GUILayout.Button("Reload", EditorStyles.toolbarButton, GUILayout.Width(60)))
                    {
                        ReloadTableData();
                    }
                }

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
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(group.Key, EditorStyles.miniLabel, GUILayout.Width(leftPanelWidth - 16));
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

                // Header is drawn outside the scroll view so its rects stay
                // in window-space coordinates, matching HandleColumnResize.
                DrawTableHeader();

                tableScroll = EditorGUILayout.BeginScrollView(tableScroll);
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
                DrawHeaderCell("Hierarchy Path", 0);
                DrawHeaderCell("PlaceHolder", 1);
                DrawHeaderCell("TextKey", 2);
                for (var i = 0; i < languages.Count; i++)
                {
                    DrawHeaderCell(languages[i], 3 + i);
                }
            }
        }

        void DrawHeaderCell(string label, int colIndex)
        {
            var width = GetColWidth(colIndex);
            EditorGUILayout.LabelField(label, EditorStyles.toolbarButton, GUILayout.Width(width));

            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            var cellRect = GUILayoutUtility.GetLastRect();
            var handleRect = new Rect(cellRect.xMax - ColHandleWidth * 0.5f, cellRect.y, ColHandleWidth, cellRect.height);

            while (colHandleRects.Count <= colIndex) { colHandleRects.Add(Rect.zero); }
            colHandleRects[colIndex] = handleRect;
        }

        void DrawTableRow(ComponentRow row)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                // Column 1: Hierarchy Path
                if (GUILayout.Button(row.hierarchyPath, leftAlignedButtonStyle, GUILayout.Width(pathColWidth)))
                {
                    SelectGameObject(row);
                }

                // Column 2: PlaceHolder (selectable)
                EditorGUILayout.SelectableLabel(row.placeholderText,
                    GUILayout.Width(placeholderColWidth),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));

                // Column 3: TextKey (yellow if unsaved)
                var keyStyle = row.IsTextKeyDirty ? dirtyTextFieldStyle : EditorStyles.textField;
                var nextKey = EditorGUILayout.TextField(row.textKey, keyStyle, GUILayout.Width(keyColWidth));
                if (nextKey != row.textKey)
                {
                    row.textKey = nextKey;
                    foreach (var lang in languages)
                    {
                        row.langData[lang] = allTsvData.TryGetValue(nextKey, out var langMap) &&
                                             langMap.TryGetValue(lang, out var text)
                            ? text
                            : string.Empty;
                    }

                    isDirty = true;
                }

                // Column 4+: Language text
                for (var i = 0; i < languages.Count; i++)
                {
                    var lang = languages[i];
                    var colW = i < langColWidths.Count ? langColWidths[i] : 160f;
                    var current = row.langData.TryGetValue(lang, out var t) ? t : string.Empty;
                    var hasKey = !string.IsNullOrEmpty(row.textKey);
                    var isReadOnly = hasKey &&
                                     keySourceMap.TryGetValue(row.textKey, out var sm) &&
                                     sm.TryGetValue(lang, out var src) &&
                                     src != selectedEntry.tsvBaseName;

                    using (new EditorGUI.DisabledScope(!hasKey || isReadOnly))
                    {
                        var next = EditorGUILayout.TextField(current, GUILayout.Width(colW));
                        if (hasKey && !isReadOnly && next != current)
                        {
                            foreach (var r in rows)
                            {
                                if (r.textKey == row.textKey)
                                {
                                    r.langData[lang] = next;
                                }
                            }

                            isDirty = true;
                        }
                    }
                }
            }

            EditorGUILayout.Space(1);
        }

        void SelectGameObject(ComponentRow row)
        {
            if (selectedEntry.isScene)
            {
                Selection.activeGameObject = row.component.gameObject;
            }
            else
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(selectedEntry.assetPath);
                if (prefab == null)
                {
                    return;
                }

                AssetDatabase.OpenAsset(prefab);
                var stage = PrefabStageUtility.GetCurrentPrefabStage();
                if (stage == null)
                {
                    return;
                }

                var go = FindGameObjectByPath(stage.prefabContentsRoot, row.hierarchyPath);
                if (go != null)
                {
                    Selection.activeGameObject = go;
                }
            }
        }

        static GameObject FindGameObjectByPath(GameObject root, string path)
        {
            var parts = path.Split('/');
            if (parts.Length == 0 || root.name != parts[0])
            {
                return null;
            }

            var current = root;
            for (var i = 1; i < parts.Length; i++)
            {
                Transform found = null;
                foreach (Transform child in current.transform)
                {
                    if (child.name == parts[i])
                    {
                        found = child;
                        break;
                    }
                }

                if (found == null)
                {
                    return null;
                }

                current = found.gameObject;
            }

            return current;
        }

        // ─────────────────────────────────────────────────────────────────
        // Check Duplicates
        // ─────────────────────────────────────────────────────────────────

        void CheckDuplicateKeys()
        {
            if (settings == null)
            {
                return;
            }

            var folder = ToAbsolutePath(settings.TextTableFolderPath);
            if (!Directory.Exists(folder))
            {
                EditorUtility.DisplayDialog("Folder Not Found",
                    $"TSV folder not found:\n{settings.TextTableFolderPath}", "OK");
                return;
            }

            // key -> set of base names (scopes) that contain this key
            var keyToScopes = new Dictionary<string, HashSet<string>>();

            foreach (var file in Directory.GetFiles(folder, "*.tsv"))
            {
                var nameNoExt = Path.GetFileNameWithoutExtension(file);
                var dot = nameNoExt.LastIndexOf('.');
                if (dot < 0)
                {
                    continue;
                }

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
                    if (!keyToScopes.ContainsKey(key))
                    {
                        keyToScopes[key] = new HashSet<string>();
                    }

                    keyToScopes[key].Add(baseName);
                }
            }

            var duplicates = keyToScopes
                .Where(kvp => kvp.Value.Count > 1)
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => (kvp.Key, kvp.Value))
                .ToList();

            if (duplicates.Count == 0)
            {
                EditorUtility.DisplayDialog("No Duplicates",
                    "No duplicate TextKeys found across scopes.", "OK");
                return;
            }

            DuplicateKeysWindow.Show(duplicates);
        }

        // ─────────────────────────────────────────────────────────────────
        // Reload
        // ─────────────────────────────────────────────────────────────────

        void ReloadTableData()
        {
            if (isDirty && !EditorUtility.DisplayDialog(
                    "Unsaved Changes",
                    "There are unsaved changes. Discard and reload?",
                    "Reload",
                    "Cancel"))
            {
                return;
            }

            LoadTableData();
            Repaint();
        }

        // ─────────────────────────────────────────────────────────────────
        // Add Language
        // ─────────────────────────────────────────────────────────────────

        void AddLanguage(string langCode)
        {
            if (settings == null)
            {
                return;
            }

            var folder = ToAbsolutePath(settings.TextTableFolderPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var baseName = selectedEntry != null ? selectedEntry.tsvBaseName : "TextTable";
            var filePath = Path.Combine(folder, $"{baseName}.{langCode}.tsv");

            if (File.Exists(filePath))
            {
                EditorUtility.DisplayDialog(
                    "Already Exists",
                    $"A file for language \"{langCode}\" already exists.",
                    "OK");
                return;
            }

            File.WriteAllText(filePath, "key\ttext\n", Encoding.UTF8);
            AssetDatabase.Refresh();
            LoadTableData();
            Repaint();
        }

        // ─────────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────────

        static string ReadTextKey(LHTextMeshPro comp)
        {
            var so = new SerializedObject(comp);
            return so.FindProperty("textKey")?.stringValue ?? string.Empty;
        }

        static string ReadPlaceholderText(LHTextMeshPro comp)
        {
            var so = new SerializedObject(comp);
            return so.FindProperty("m_text")?.stringValue ?? string.Empty;
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

        class DuplicateKeysWindow : EditorWindow
        {
            List<(string key, HashSet<string> scopes)> duplicates;
            Vector2 scroll;

            public static void Show(List<(string, HashSet<string>)> duplicates)
            {
                var window = GetWindow<DuplicateKeysWindow>(true, "Duplicate TextKeys", true);
                window.duplicates = duplicates;
                window.minSize = new Vector2(440, 300);
            }

            void OnGUI()
            {
                EditorGUILayout.LabelField(
                    $"{duplicates.Count} duplicate key(s) found across scopes:",
                    EditorStyles.boldLabel);
                EditorGUILayout.Space(4);

                scroll = EditorGUILayout.BeginScrollView(scroll);

                foreach (var (key, scopes) in duplicates)
                {
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        EditorGUILayout.LabelField(key, EditorStyles.boldLabel);
                        EditorGUILayout.LabelField(
                            $"Scopes: {string.Join(", ", scopes.OrderBy(s => s))}",
                            EditorStyles.miniLabel);
                    }

                    EditorGUILayout.Space(2);
                }

                EditorGUILayout.EndScrollView();
            }
        }

        class AddLanguagePopup : PopupWindowContent
        {
            string languageCode = string.Empty;
            readonly Action<string> onConfirm;
            bool focusRequested;

            public AddLanguagePopup(Action<string> onConfirm)
            {
                this.onConfirm = onConfirm;
            }

            public override Vector2 GetWindowSize() => new Vector2(240, 68);

            public override void OnGUI(Rect rect)
            {
                EditorGUILayout.LabelField("Language code (e.g. ja, en):");

                GUI.SetNextControlName("LangCodeField");
                languageCode = EditorGUILayout.TextField(languageCode);

                if (!focusRequested)
                {
                    EditorGUI.FocusTextInControl("LangCodeField");
                    focusRequested = true;
                }

                EditorGUILayout.Space(2);

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(languageCode)))
                    {
                        if (GUILayout.Button("OK"))
                        {
                            Confirm();
                        }
                    }

                    if (GUILayout.Button("Cancel"))
                    {
                        editorWindow.Close();
                    }
                }

                if (Event.current.type == EventType.KeyDown &&
                    Event.current.keyCode == KeyCode.Return &&
                    !string.IsNullOrWhiteSpace(languageCode))
                {
                    Confirm();
                    Event.current.Use();
                }
            }

            void Confirm()
            {
                onConfirm?.Invoke(languageCode.Trim());
                editorWindow.Close();
            }
        }

        class ComponentRow
        {
            public readonly string hierarchyPath;
            public readonly string placeholderText;
            public string textKey;
            string savedTextKey;
            public readonly LHTextMeshPro component;
            public readonly Dictionary<string, string> langData;

            public bool IsTextKeyDirty => textKey != savedTextKey;

            public ComponentRow(string hierarchyPath, string textKey, string placeholderText,
                LHTextMeshPro component, Dictionary<string, string> langData)
            {
                this.hierarchyPath = hierarchyPath;
                this.textKey = textKey;
                this.savedTextKey = textKey;
                this.placeholderText = placeholderText;
                this.component = component;
                this.langData = langData;
            }

            public void MarkSaved() => savedTextKey = textKey;
        }
    }
}
