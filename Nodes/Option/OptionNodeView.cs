using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes.Option
{
    class OptionNodeView : Node, ILogicNodeView
    {
        public ILogicNode AsLogicNode { get; set; }
        public OptionNode node { get; set; }
        public Port InputLogic { get; set; }
        public Port OutputLogic { get; set; }

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
            container.Add(new Label("Текст"));
            container.Add(OutputLogic);
        }

        public void CreateInputLogicPort()
        {
            InputLogic = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            InputLogic.name = "LogicPort";
            InputLogic.portName = "";
            InputLogic.portColor = new Color(1, 0.5f, 0);
            
            titleContainer.Insert(0, InputLogic);
        }

        public void CreateOutputLogicPort()
        {
            OutputLogic = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            OutputLogic.name = "LogicPort";
            OutputLogic.portName = "";
            OutputLogic.portColor = new Color(1, 0.5f, 0);

            titleContainer.Add(OutputLogic);
        }
    }
}
