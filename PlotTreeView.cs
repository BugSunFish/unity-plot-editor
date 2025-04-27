using Assets.Scripts.ScenarioSystem.Nodes;
using Assets.Scripts.ScenarioSystem.Nodes.Dialogue;
using Assets.unity_plot_editor.Nodes.Abstractions;
using Assets.unity_plot_editor.Nodes.Abstractions.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlotTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<PlotTreeView, UxmlTraits> { }
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

        // Отрисовываем все ноды 
        tree.ScenarioNodes.ForEach(node => CreateNodeView(node));

        // Отрисовываем все связи
        tree.ScenarioNodes.ForEach(n =>
        {

            PlotNodeView view = FindScenarioNodeView(n);

            // Отрисовываем главные нормальные эджи
            if (view is INormalNodeView parentNormalView)
            {
                foreach (var childNormalNode in tree.GetChildren(view.node as INormalNode))
                {
                    PlotNodeView childNormalView = FindScenarioNodeView(childNormalNode as PlotNode);

                    Edge edge = parentNormalView?.OutputNormal.ConnectTo((childNormalView as INormalNodeView)?.InputNormal);

                    AddElement(edge);
                }
            }

            // Отрисовываем главные логические эджи
            if (view is ILogicNodeView parentLogicView)
            {
                var childLogicNode = (view.node as ILogicNode).LogicNode;

                PlotNodeView childLogicView = FindScenarioNodeView(childLogicNode as PlotNode);

                Edge edge = parentLogicView?.OutputLogic.ConnectTo((childLogicView as ILogicNodeView)?.InputLogic);

                AddElement(edge);
            }

            ///////////////////

            //if (parentView is ILogicNodeView logicNodeChildView && logicNodeChildView.AsLogicNode.LogicNode != null)
            //{

            //    var logicNodeParentView = FindScenarioNodeView(logicNodeChildView.AsLogicNode.LogicNode as PlotNode) as ILogicNodeView;

            //    Edge edge = logicNodeParentView.OutputLogic.ConnectTo(logicNodeChildView.InputLogic);

            //    AddElement(edge);

            //    /*
            //     * сомнительный функционал, соединяет ноды неправильно
            //     * 
            //    if (logicNodeChildView is DialogueNodeView dialogueNodeView)
            //    {
            //        foreach (ILogicNode optionNodeView in dialogueNodeView.OptionViews)
            //        {
            //            var optionLogicNodeViewChild = FindLogicNodeView(optionNodeView);
            //            var optionLogicNodeViewParent = FindLogicNodeView(optionNodeView.LogicNode);

            //            Edge edgeOptions = optionLogicNodeViewParent.OutputLogic.ConnectTo(optionLogicNodeViewChild.InputLogic);

            //            AddElement(edgeOptions);
            //        }
            //    }
            //    */
            //}
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

        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                PlotNodeView nodeView = elem as PlotNodeView;

                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                //Edge edge = elem as Edge;

                //if (edge != null)
                //{
                //    PlotNodeView parentView = edge.output.node as PlotNodeView;
                //    PlotNodeView childView = edge.input.node as PlotNodeView;

                //    // какая та проблема именно с соедиеннием

                //    if (edge.input.name == "LogicPort")
                //    {
                //        if (parentView.node is ILogicNode parent && childView.node is ILogicNode child)
                //        {
                //            child.ClearLogic();
                //        }
                //    }
                //    else if (edge.input.name == "NormalPort")
                //    {
                //        tree.RemoveChild(parentView.node, childView.node);
                //    }

                //}

            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                // Создние нормального соединения
                if (edge.input.name == "NormalPort" && edge.output.node is PlotNodeView parentNormalView && edge.input.node is PlotNodeView childNormalView)
                {
                    tree.AddChild(parentNormalView.node as INormalNode, childNormalView.node as INormalNode);
                }

                // Создние логического соединения
                if (edge.input.name == "LogicPort" && edge.output.node is PlotNodeView parentLogicView && edge.input.node is PlotNodeView childLogicView)
                {
                    // tree.AddLogicChild(parentLogicView.node as ILogicNode, childLogicView.node as ILogicNode);
                }

                // А как соединять опшны?

                //if (edge.input.name == typeof(LogicPort).Name && edge.output.node is ILogicNodeView parentLogicView && edge.input.node is ILogicNodeView childLogicView)
                //{

                //    if (parentLogicView.AsLogicNode is ILogicNode parent && childLogicView.AsLogicNode is ILogicNode child)
                //    {

                //    }
                //    child.SetLogic(parent);
                //}
                //else if (edge.input.name == "NormalPort" && edge.output.node is PlotNodeView parentView && edge.input.node is PlotNodeView childView)
                //{
                //    tree.AddChild(parentView.node, childView.node);
                //}

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