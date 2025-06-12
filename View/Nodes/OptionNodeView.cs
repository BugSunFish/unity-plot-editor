using Assets.Scripts.ScenarioSystem.Nodes.Dialogue;
using Assets.unity_plot_editor.Nodes.Abstractions;
using Assets.unity_plot_editor.Nodes.Abstractions.Ports;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes.Option
{
    public class OptionNodeView : PlotNodeView, INormalNodeView, ILogicNodeView
    {
        public OptionNode Node { get => node as OptionNode; }

        public NormalPort InputNormal { get; set; }
        public NormalPort OutputNormal { get; set; }

        public LogicPort InputLogic { get; set; }
        public LogicPort OutputLogic { get; set; }

        public TemplateContainer container { get; set; }

        public OptionNodeView(OptionNode option)
        {
            node = option;
            title = option.Title;
            viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputLogicPort();
            CreateOutputLogicPort();

            container = new TemplateContainer();
            container.style.flexDirection = FlexDirection.Row;
            container.style.justifyContent = Justify.SpaceBetween;

            container.Add(InputLogic);
            var label = new Label(Node.Title + " " + Node.Guid);
            label.style.color = Color.white;
            container.Add(label);
            container.Add(OutputLogic);
        }

        public void CreateInputNormalPort()
        {
            throw new NotImplementedException();
        }

        public void CreateOutputNormalPort()
        {
            throw new NotImplementedException();
        }

        public void CreateInputLogicPort()
        {
            InputLogic = LogicPort.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            InputLogic.portName = "";
            InputLogic.portColor = new Color(1, 0.5f, 0);
            
            titleContainer.Insert(0, InputLogic);
        }

        public void CreateOutputLogicPort()
        {
            OutputLogic = LogicPort.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            OutputLogic.portName = "";
            OutputLogic.portColor = new Color(1, 0.5f, 0);

            titleContainer.Add(OutputLogic);
        }

        public Port GetPortByGuid(Guid guid)
        {
            if (InputNormal.Guid == guid)
                return InputNormal;

            if (OutputNormal.Guid == guid)
                return OutputNormal;

            if (InputLogic.Guid == guid)
                return InputLogic;

            if (OutputLogic.Guid == guid)
                return OutputLogic;

            return null;
        }

        public ILogicNode GetLogicNodeByPortGuid(Guid guid)
        {
            Debug.Log(
                "public ILogicNode GetLogicNodeByPortGuid(Guid guid)\n\n" +
                $"guid: {guid}\n" +
                $"InputLogic.Guid: {InputLogic.Guid}\n" +
                $"OutputLogic.Guid: {OutputLogic.Guid}\n");

            if (InputLogic.Guid == guid)
                return Node;

            if (OutputLogic.Guid == guid)
                return Node;

            return null;
        }

        public INormalNode GetNormalNodeByPortGuid(Guid guid)
        {
            return null;
        }

        public ILogicNode GetLogicNodeByNodeGuid(string guid)
        {
            if (node.guid == guid)
                return Node;

            return null;
        }
    }
}
