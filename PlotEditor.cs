using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class PlotEditor : EditorWindow
{
    PlotTreeView treeView;

    [MenuItem("Plot Editor/Editor")]
    public static void ShowExample()
    {
        PlotEditor wnd = GetWindow<PlotEditor>();
        wnd.titleContent = new GUIContent("PlotEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/unity-plot-editor/PlotEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/unity-plot-editor/PlotEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<PlotTreeView>();

        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        PlotTree tree = Selection.activeObject as PlotTree;

        if (tree)
        {
            treeView.DrawView(tree);
        }
    }
}