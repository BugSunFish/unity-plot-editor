using Assets.unity_plot_editor.Nodes.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes
{
    public class OptionNode : PlotNode, INormalNode, ILogicNode
    {
        // message
        public string Title { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }

        // normal
        public List<INormalNode> ChildNodes { get; set; }

        // logic
        public bool FromData { get; set; } // isActive
        public ILogicNode LogicNode { get; set; } // wasActive
        public string Guid => guid;


        public OptionNode(string author, string title, string message)
        {
            Author = author;
            Title = title;
            Message = message;
            ChildNodes = new List<INormalNode>();
        }

        public void SetLogic(ILogicNode logicNode) => LogicNode = logicNode;

        public void ClearLogic() => LogicNode = null;

        public bool GetLogic() => LogicNode != null ? LogicNode.FromData : true;

        public void AddChild(INormalNode node) => ChildNodes.Add(node);

        public void RemoveChild(INormalNode node) => ChildNodes.Remove(node);

    }
}
