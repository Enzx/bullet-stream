using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletSteam.GameFramework.Graph.Editor
{
    public class StateMachineEditorWindow : EditorWindow
    {
        private const string LastOpenedGraph = "StateMachineEditorWindow.lastOpen";
        [SerializeField] private VisualTreeAsset _graphViewAsset;
        [SerializeField] private StyleSheet _graphViewStyle;

        private StateMachineGraph _graphView;
        private Toolbar _toolbar;
        private ToolbarMenu _toolbarMenu;
        private VisualElement _cover;

        [MenuItem("Window/State Machine Editor")]
        public static void OpenWindow()
        {
            _ = GetWindow<StateMachineEditorWindow>();
        }

        private void CreateGUI()
        {
            titleContent = new GUIContent("State Machine Editor");
            _graphViewAsset.CloneTree(rootVisualElement);

            _cover = rootVisualElement.Q<VisualElement>("cover");
            _cover.style.display = DisplayStyle.Flex;
            //Set up the graph view
            _graphView = rootVisualElement.Q<StateMachineGraph>();
            _graphView.styleSheets.Add(_graphViewStyle);
            _graphView.Init(this);
            //Set up the toolbar
            _toolbar = rootVisualElement.Q<Toolbar>();
            _toolbarMenu = _toolbar.Q<ToolbarMenu>();
            _toolbarMenu.menu.AppendAction("Open", OpenGraph);
            _toolbarMenu.menu.AppendAction("New", NewGraph);
            _toolbarMenu.menu.AppendAction("Save", SaveGraph);

            OpenLastGraph();
        }

        private void OpenLastGraph()
        {
            string path = EditorPrefs.GetString(LastOpenedGraph, string.Empty);
            if (string.IsNullOrEmpty(path)) return;
            StateMachineData data = AssetDatabase.LoadAssetAtPath<StateMachineData>(path);
            if (data == null) return;
            OpenGraph(data);
        }

        private void OnDestroy()
        {
            _graphView.UnInit();
            _graphView.Clear();
        }

        private void OpenGraph(DropdownMenuAction action)
        {
            string path = EditorUtility.OpenFilePanel("Open State Machine", "Assets", "asset");
            if (string.IsNullOrEmpty(path)) return;
            // make path relative to the project
            path = path.Replace(Application.dataPath, "Assets");
            StateMachineData data = AssetDatabase.LoadAssetAtPath<StateMachineData>(path);
            if (data == null) return;
            OpenGraph(data);
            EditorPrefs.SetString(LastOpenedGraph, path);
        }

        private void OpenGraph(StateMachineData data)
        {
            SerializedObject so = new(data);
            rootVisualElement.Bind(so);
            _graphView.SetGraphData(data);
            Label label = _toolbar.Q<Label>("graph-name-label");
            label.BindProperty(so.FindProperty("m_Name"));
            _cover.style.display = DisplayStyle.None;
        }


        private void SaveGraph(DropdownMenuAction action)
        {
            if (_graphView.GraphData == null) return;
            _graphView.Serialize();
            EditorUtility.SetDirty(_graphView.GraphData);
            for (int i = 0; i < _graphView.GraphData.NodesData.Count; i++)
            {
                EditorUtility.SetDirty(_graphView.GraphData.NodesData[i]);
            }

            AssetDatabase.SaveAssets();
        }

        private void NewGraph(DropdownMenuAction action)
        {
            StateMachineData data = CreateInstance<StateMachineData>();
            //Open Save File Dialog
            string path = EditorUtility.SaveFilePanelInProject("Save State Machine", "New State Machine", "asset",
                "Save State Machine", "Assets");
            if (string.IsNullOrEmpty(path)) return;
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssetIfDirty(data);
            _graphView.SetGraphData(data);
            _cover.style.display = DisplayStyle.None;
        }
    }
}