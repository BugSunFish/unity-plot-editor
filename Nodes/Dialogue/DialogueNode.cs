using Assets.Scripts.ScenarioSystem.Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : PlotNode, ILogicNode
{
    // диалог
    public bool IsRepeats { get; set; }
    public override List<PlotNode> ChildNodes { get => childNodes; set => childNodes = value; }
    public List<PlotNode> NextNodes
    {
        get
        {
            if (IsRepeats)
            {
                var nodes = new List<PlotNode>(childNodes);
                nodes.Add(this);
                return nodes;
            }
            return childNodes;
        }
        set
        {
            childNodes = value;
        }
    }
    public HashSet<string> Authors { get; set; }
    public List<OptionNode> Options { get; set; }
    public OptionNode StartNode { get; set; }


    private List<PlotNode> childNodes;


    // логика
    public bool FromData { get; set; }
    public ILogicNode LogicNode { get; set; }


    public DialogueNode()
    {
        childNodes = new List<PlotNode>();
        Authors = new HashSet<string>();
        IsRepeats = false;

        Options = new List<OptionNode>();
    }

    public void AddMessage(OptionNode parent, OptionNode child)
    {
        parent.AddChild(child);
        if (parent.ChildNodes.Count > 1)
        {
            Options.Add(child);
        }
    }

    public void RemoveMessage(OptionNode parent, OptionNode child)
    {
        parent.RemoveChild(child);
        if (parent.ChildNodes.Count <= 1)
        {
            Options.Remove(child);
        }
    }

    public void SetLogic(ILogicNode logicNode) => LogicNode = logicNode;

    public void ClearLogic() => LogicNode = null;

    public bool GetLogic() => LogicNode != null ? LogicNode.FromData : true;

    public override void AddChild(PlotNode node) => childNodes.Add(node);

    public override void RemoveChild(PlotNode node) => childNodes.Remove(node);
    
}
