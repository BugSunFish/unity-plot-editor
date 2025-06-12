using Assets.Scripts.ScenarioSystem.Nodes;
using Assets.Scripts.ScenarioSystem.Nodes.Dialogue;
using Assets.Scripts.ScenarioSystem.Nodes.Option;
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

    public PlotNodeView FindScenarioNodeView(PlotNode node)
    {
        return GetNodeByGuid(node.guid) as PlotNodeView;
    }

    public ILogicNodeView FindLogicNodeViewByGuid(string guid)
    {
        return nodes.ToList()
            .Where(x => (x as DialogueNodeView) != null)
            .Select(x => (x as DialogueNodeView).GetLogicNodeViewByNodeGuid(guid))
            .Where(x => x != null)
            .FirstOrDefault();
    }

    public ILogicNode FindLogicNodeByGuid(string guid)
    {
        return nodes.ToList()
            .Where(x => (x as DialogueNodeView) != null)
            .Select(x => (x as ILogicNodeView).GetLogicNodeByNodeGuid(guid))
            .Where(x => x != null)
            .FirstOrDefault();
    }

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

            // Отрисовка логических связей
            if (view is ILogicNodeView parentLogicView)
            {
                var childLogicNode = (view.node as ILogicNode).LogicNode;

                Debug.Log(
                        $"Поддержка логической связи\n" +
                        $"child logic node: {childLogicNode != null}\n" +
                        $"type: {childLogicNode?.GetType().Name}\n"
                        );

                if (childLogicNode != null)
                {
                    PlotNodeView childLogicView = FindLogicNodeViewByGuid((childLogicNode as PlotNode).guid) as PlotNodeView;

                    Debug.Log(
                        $"Отрисовка логической связи\n" +
                        $"parent: {parentLogicView != null}\n" +
                        $"child: {childLogicView != null}\n" +
                        $"parent guid: {(parentLogicView as PlotNodeView)?.node.guid}\n" +
                        $"child guid: {childLogicView?.node.guid}\n"
                        );

                    Edge edge = (childLogicView as ILogicNodeView)?.OutputLogic.ConnectTo(parentLogicView?.InputLogic);

                    if (edge != null)
                    {
                        Debug.Log(
                            $"Отрисовка логической связи прошла успешно\n" +
                            $"parent guid: {(parentLogicView as PlotNodeView).node.guid}\n" +
                            $"child guid: {childLogicView.node.guid}\n"
                            );
                    }

                    AddElement(edge);
                }

                if (view is DialogueNodeView dialogueNodeView)
                {
                    Debug.Log(
                        $"Отрисовка логических option связей DialogueNode\n" +
                        $"parent guid: {(parentLogicView as PlotNodeView).node.guid}\n" +
                        $"options count: {dialogueNodeView.OptionViews.Count}"
                        );

                    foreach (var optionView in dialogueNodeView.OptionViews)
                    {
                        Debug.Log(
                            $"children option:\n" +
                            $"guid: {optionView.Node.Guid}\n"
                            );

                        var parentLogicOptionNode = optionView.Node.LogicNode;
                        
                        if (parentLogicOptionNode != null)
                        {
                            Debug.Log("Найдена связь");

                            ILogicNodeView parentLogicOptionView = FindLogicNodeViewByGuid((parentLogicOptionNode as PlotNode).guid);
                            Debug.Log(
                                $"parent option:\n" +
                                $"guid: {(parentLogicOptionNode as PlotNode)?.guid}\n" +
                                $"view: {(parentLogicOptionView != null ? "Найден" : "Не найден")} \n"
                                );

                            ILogicNodeView childLogicOptionView = optionView;

                            Debug.Log(
                                $"Отрисовка логической связи\n" +
                                $"parent: {parentLogicView != null}\n" +
                                $"child: {childLogicOptionView != null}\n" +
                                $"parent guid: {(parentLogicView as PlotNodeView)?.node.guid}\n" +
                                $"child guid: {(childLogicOptionView as PlotNodeView).node.guid}\n"
                                );

                            Edge edge = parentLogicOptionView?.OutputLogic.ConnectTo(childLogicOptionView?.InputLogic);

                            if (edge != null)
                            {
                                Debug.Log("Отрисовка логической option связи прошла успешно");
                            }

                            AddElement(edge);
                        }
                        else
                        {
                            Debug.Log("Логическая связь option.LogicNode равна null");
                        }
                    }
                }
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

        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {

                // TODO: Добавить потом удаление привязанных эджей

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
                        if (childView.node is ILogicNode child)
                        {
                            child.ClearLogic();
                        }
                    }

                    if (edge.input.name == "NormalPort")
                    {
                        tree.RemoveChild(parentView.node as INormalNode, childView.node as INormalNode);
                    }

                }

            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {

                /* INFO:
                 На вход мы получаем edge с портами для связи которые можем понимать как наши порты с ID
                 */

                // Создние нормального соединения
                if (edge.input.name == "NormalPort" && edge.output.node is PlotNodeView parentNormalView && edge.input.node is PlotNodeView childNormalView)
                {
                    var parentNode = (parentNormalView as INormalNodeView).GetNormalNodeByPortGuid((edge.output as NormalPort).Guid);
                    var childNode = (childNormalView as INormalNodeView).GetNormalNodeByPortGuid((edge.input as NormalPort).Guid);

                    Debug.Log(
                        $"Поиск нод по портам для нормальной связи\n" +
                        $"parent: {parentNode}\t" +
                        $"child: {childNode}\n"
                        );

                    tree.AddChild(parentNode, childNode);
                    Debug.Log(
                        $"Добавлена нормальная связь\n" +
                        $"Детей: {parentNormalView.childCount}\n"
                        );
                }

                // Создние логического соединения
                if (edge.input.name == "LogicPort" && edge.output.node is PlotNodeView parentLogicView && edge.input.node is PlotNodeView childLogicView)
                {
                    var parentNode = (parentLogicView as ILogicNodeView).GetLogicNodeByPortGuid((edge.output as LogicPort).Guid);
                    var childNode = (childLogicView as ILogicNodeView).GetLogicNodeByPortGuid((edge.input as LogicPort).Guid);

                    Debug.Log(
                        $"Поиск нод по портам для логической связи\n" +
                        $"parent: {parentNode.GetType().Name}\t" +
                        $"child: {childNode.GetType().Name}\n"
                        );

                    tree.AddChild(parentNode, childNode);
                    Debug.Log(
                        $"Добавлена логическая связь\n" +
                        $"Соединение: {childNode.LogicNode != null}\n"
                        );
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
        evt.menu.AppendAction($"Dialogue Node With Options", (a) => CreateDialogueNodeWithOptions());
        evt.menu.AppendAction($"StartLine Node", (a) => CreateStartLineNode());

        evt.menu.AppendAction($"Info", (a) =>
        {
            Debug.Log(
                    $"ScenarioNodes Count:\t{tree.ScenarioNodes.Count}\n" +
                    $"ScenarioRootNodes Count:\t{tree.ScenarioRootNodes.Count}\n"
                    );
        });
    }

    public void CreateDialogueNode()
    {
        DialogueNode node = tree.CreateNode(typeof(DialogueNode)) as DialogueNode;
        CreateNodeView(node);
    }
    public void CreateDialogueNodeWithOptions()
    {
        DialogueNode node = tree.CreateNode(typeof(DialogueNode)) as DialogueNode;

        OptionNode optionStart = new OptionNode("N1", null, "Hello!");
        OptionNode option0 = new OptionNode("P1", "Hello", "Hello!");
        OptionNode option1 = new OptionNode("P1", "Bye", "Bye!");

        node.StartNode = optionStart;
        node.AddMessage(optionStart, option0);
        node.AddMessage(optionStart, option1);

        CreateNodeView(node);

        AddElement(new OptionNodeView(optionStart));
        AddElement(new OptionNodeView(option0));
        AddElement(new OptionNodeView(option1));
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

        Debug.Log(
            $"Создан View для {node.GetType().Name}\n" +
            $"guid: {node.guid}\n" +
            $"position: {node.position} \n" +
            $"childrens: {(node as DialogueNode)?.ChildNodes.Count}\n"
            );
    }

}