using Assets.Scripts.ScenarioSystem.Nodes;
using Assets.unity_plot_editor.Nodes.Abstractions;
using System;
using System.Collections.Generic;
using UnityEditor;
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

        return node;
    }

    public void DeleteNode(PlotNode node)
    {
        ScenarioNodes.Remove(node);

        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(INormalNode parent, INormalNode child)
    {
        parent.AddChild(child);
    }

    public void RemoveChild(INormalNode parent, INormalNode child)
    {
        parent.RemoveChild(child);
    }

    public List<INormalNode> GetChildren(INormalNode parent) => parent.ChildNodes;
}
