using Assets.Scripts.ScenarioSystem.Nodes;
using Assets.Scripts.ScenarioSystem.Nodes.Dialogue;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class PlotTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<PlotTreeView, GraphView.UxmlTraits> { }
    public PlotTree tree;

    public PlotTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/unity-plot-editor/PlotEditor.uss");
        styleSheets.Add(styleSheet);

        tree = new PlotTree();
    }

    public PlotNodeView FindScenarioNodeView(PlotNode node) => GetNodeByGuid(node.guid) as PlotNodeView;

    public void DrawView(PlotTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        tree.ScenarioNodes.ForEach(node => CreateNodeView(node));

        tree.ScenarioNodes.ForEach(n =>
        {

            PlotNodeView parentView = FindScenarioNodeView(n);

            if (parentView is PlotNodeView)
            {
                foreach (var c in tree.GetChildren(n))
                {
                    PlotNodeView childView = FindScenarioNodeView(c);

                    Edge edge = parentView?.Output.ConnectTo(childView?.Input);

                    AddElement(edge);
                }
            }

            if (parentView is ILogicNodeView logicNodeChildView && logicNodeChildView.AsLogicNode.LogicNode != null)
            {

                var logicNodeParentView = FindScenarioNodeView(logicNodeChildView.AsLogicNode.LogicNode as PlotNode) as ILogicNodeView;

                Edge edge = logicNodeParentView.OutputLogic.ConnectTo(logicNodeChildView.InputLogic);

                AddElement(edge);
            }
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node &&
        endPort.name == startPort.name).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {

        //List<ScenarioNodeView> nodes = new List<ScenarioNodeView>();

        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                PlotNodeView nodeView = elem as PlotNodeView;

                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;

                if (edge != null)
                {
                    PlotNodeView parentView = edge.output.node as PlotNodeView;
                    PlotNodeView childView = edge.input.node as PlotNodeView;

                    if (edge.input.name == "LogicPort")
                    {
                        if (parentView.node is ILogicNode parent && childView.node is ILogicNode child)
                        {
                            child.ClearLogic();
                        }
                    }
                    else if (edge.input.name == "NormalPort")
                    {
                        tree.RemoveChild(parentView.node, childView.node);
                    }

                }

            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                PlotNodeView parentView = edge.output.node as PlotNodeView;
                PlotNodeView childView = edge.input.node as PlotNodeView;

                if (edge.input.name == "LogicPort")
                {
                    if (parentView.node is ILogicNode parent && childView.node is ILogicNode child)
                    {
                        child.SetLogic(parent);
                    }
                }
                else if (edge.input.name == "NormalPort")
                {
                    tree.AddChild(parentView.node, childView.node);
                }

            });
        }

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction($"Dialogue Node", (a) => CreateDialogueNode());
        evt.menu.AppendAction($"StartLine Node", (a) => CreateStartLineNode());

        evt.menu.AppendAction($"Info", (a) =>
        {
            Debug.Log("" +
                    $"ScenarioNodes Count:\t{tree.ScenarioNodes.Count}\n" +
                    $"ScenarioRootNodes Count:\t{tree.ScenarioRootNodes.Count}\n" +
                    "");
        });
    }

    public void CreateDialogueNode()
    {
        DialogueNode node = tree.CreateNode(typeof(DialogueNode)) as DialogueNode;
        CreateNodeView(node);
    }
    public void CreateStartLineNode()
    {
        StartLineNode node = tree.CreateNode(typeof(StartLineNode)) as StartLineNode;
        CreateNodeView(node);
    }
    public void CreateNodeView(PlotNode node)
    {
        if (node is DialogueNode dialogueNode)
            AddElement(new DialogueNodeView(dialogueNode));
        if (node is StartLineNode startLineNode)
            AddElement(new StartLineNodeView(startLineNode));
    }

}