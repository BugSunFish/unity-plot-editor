using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes
{
    public class OptionNode : PlotNode, ILogicNode
    {
        // сообщения
        public string Title { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public override List<PlotNode> ChildNodes { get; set; }
        

        // логика
        public bool FromData { get; set; } // isActive
        public ILogicNode LogicNode { get; set; } // wasActive

        public OptionNode(string title)
        {
            Title = title;
        }

        public void SetLogic(ILogicNode logicNode) => LogicNode = logicNode;

        public void ClearLogic() => LogicNode = null;

        public bool GetLogic() => LogicNode != null ? LogicNode.FromData : true;

        public override void AddChild(PlotNode node) => ChildNodes.Add(node);

        public override void RemoveChild(PlotNode node) => ChildNodes.Remove(node);

    }
}
