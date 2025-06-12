using Assets.Scripts.ScenarioSystem.Nodes;
using Assets.unity_plot_editor.Nodes.Abstractions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : PlotNode, INormalNode, ILogicNode
{
    // dialogue
    public bool IsRepeats { get; set; }
    public OptionNode StartNode { get; set; }
    public HashSet<string> Authors { get; set; }
    public HashSet<OptionNode> Options { get; set; }

    // normal
    public List<INormalNode> ChildNodes { get; set; }
    public List<INormalNode> NextNodes
    {
        get
        {
            if (IsRepeats)
            {
                var nodes = new List<INormalNode>(ChildNodes);
                nodes.Add(this);
                return nodes;
            }
            return ChildNodes;
        }
        set
        {
            ChildNodes = value;
        }
    }


    // logic
    public bool FromData { get; set; }
    public ILogicNode LogicNode { get; set; }
    public string Guid => guid;


    public DialogueNode()
    {
        ChildNodes = new List<INormalNode>();
        Authors = new HashSet<string>();
        IsRepeats = false;

        Options = new HashSet<OptionNode>();
    }

    public void AddMessage(OptionNode parent, OptionNode child)
    {
        parent.AddChild(child);
        if (parent.ChildNodes.Count > 1)
        {
            Options.Add(parent);
        }
    }

    public void RemoveMessage(OptionNode parent, OptionNode child)
    {
        parent.RemoveChild(child);
        if (parent.ChildNodes.Count <= 1)
        {
            Options.Remove(parent);
        }
    }

    public void SetLogic(ILogicNode logicNode) => LogicNode = logicNode;

    public void ClearLogic() => LogicNode = null;

    public bool GetLogic() => LogicNode != null ? LogicNode.FromData : true;

    public void AddChild(INormalNode node) => ChildNodes.Add(node);

    public void RemoveChild(INormalNode node) => ChildNodes.Remove(node);

}
