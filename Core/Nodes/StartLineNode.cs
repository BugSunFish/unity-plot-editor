using Assets.Scripts.ScenarioSystem.Nodes;
using Assets.unity_plot_editor.Nodes.Abstractions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLineNode : PlotNode, INormalNode
{
    // линия диалога
    public List<INormalNode> ChildNodes { get; set; }

    public StartLineNode()
    {
        ChildNodes = new List<INormalNode>();
    }

    public void AddChild(INormalNode node) => ChildNodes.Add(node);

    public void RemoveChild(INormalNode node) => ChildNodes.Remove(node);
}
