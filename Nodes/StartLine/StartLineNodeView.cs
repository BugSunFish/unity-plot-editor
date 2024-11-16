using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes.Dialogue
{
    public class StartLineNodeView : PlotNodeView
    {

        public StartLineNode Node { get => node as StartLineNode; }

        public StartLineNodeView(StartLineNode startLineNode) 
        { 
            node = startLineNode;
            title = "Start Line";
            viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateOutputPorts();
        }

        public void CreateOutputPorts()
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            Output.name = "NormalPort";
            Output.portName = "";
            Output.portColor = Color.white;

            titleContainer.Add(Output);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            evt.menu.AppendAction($"Info", (a) => 
            {
                Debug.Log("" +
                    $"Child Nodes Count:\t{Node.ChildNodes.Count}\n" +
                    "");
            });
        }

    }
}
