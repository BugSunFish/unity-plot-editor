using Assets.Scripts.ScenarioSystem.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLineNode : PlotNode
{
    // линия диалога
    public override List<PlotNode> ChildNodes { get; set; }

    public StartLineNode()
    {
        ChildNodes = new List<PlotNode>();
    }

    public override void AddChild(PlotNode node) => ChildNodes.Add(node);

    public override void RemoveChild(PlotNode node) => ChildNodes.Remove(node);
}
