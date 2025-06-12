using Assets.Scripts.ScenarioSystem.Nodes;
using Assets.unity_plot_editor.Nodes.Abstractions;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu]
public class PlotTree : ScriptableObject
{
    public List<PlotNode> ScenarioNodes { get; set; }
    public List<StartLineNode> ScenarioRootNodes { get; set; }

    public PlotTree()
    {
        ScenarioNodes = new List<PlotNode>();
        ScenarioRootNodes = new List<StartLineNode>();
    }

    public PlotNode CreateNode(System.Type type)
    {
        PlotNode node = CreateInstance(type) as PlotNode;

        node.name = type.Name;
        node.guid = Guid.NewGuid().ToString();
        ScenarioNodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        Debug.Log(
            $"Создана нода {type}\n" +
            $"guid: {node.guid}\n" +
            $"position: {node.position}\n"
            );
        return node;
    }

    public void DeleteNode(PlotNode node)
    {
        ScenarioNodes.Remove(node);

        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();

        Debug.Log(
            $"Удалена нода {node.GetType().Name}\n" +
            $"guid: {node.guid}\n"
            );
    }

    public void AddChild(INormalNode parent, INormalNode child)
    {
        parent.AddChild(child);
        Debug.Log(
            $"Добавлен ребёнок {child.GetType().Name}\n" +
            $"Детей: {parent?.ChildNodes.Count}\n"
            );
    }

    public void RemoveChild(INormalNode parent, INormalNode child)
    {
        parent.RemoveChild(child);
        Debug.Log(
            $"Удалён ребёнок {child.GetType().Name}\n" +
            $"Детей: {parent?.ChildNodes.Count}\n"
            );
    }

    public void AddChild(ILogicNode parent, ILogicNode child)
    {
        child.SetLogic(parent);
        Debug.Log(
            $"Установлена логика {child.GetType().Name}\n" +
            $"Родительская логика для child: {child.LogicNode.GetType().Name}\n"
            );  
    }

    public void RemoveChild(ILogicNode child)
    {
        child.ClearLogic();
        Debug.Log(
            $"Удалена логика {child.GetType().Name}\n" +
            $"Родительская логика для child: {child.LogicNode?.GetType().Name}\n"
            );
    }

    public List<INormalNode> GetChildren(INormalNode parent) => parent.ChildNodes;
}
