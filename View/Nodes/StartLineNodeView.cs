using Assets.unity_plot_editor.Nodes.Abstractions;
using Assets.unity_plot_editor.Nodes.Abstractions.Ports;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes.Dialogue
{
    public class StartLineNodeView : PlotNodeView, INormalNodeView
    {
        public StartLineNode Node { get => node as StartLineNode; }

        public NormalPort InputNormal { get; set; }
        public NormalPort OutputNormal { get; set; }

        public StartLineNodeView(StartLineNode startLineNode) 
        { 
            node = startLineNode;
            title = "Start Line";
            viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateOutputNormalPort();
        }

        public void CreateInputNormalPort()
        {
            throw new NotImplementedException();
        }

        public void CreateOutputNormalPort()
        {
            OutputNormal = NormalPort.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            OutputNormal.name = "NormalPort";
            OutputNormal.portName = "";
            OutputNormal.portColor = Color.white;

            titleContainer.Add(OutputNormal);
        }

        public Port GetPortByGuid(Guid guid)
        {
            throw new NotImplementedException();
        }

        public INormalNode GetNormalNodeByPortGuid(Guid guid)
        {
            throw new NotImplementedException();
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
