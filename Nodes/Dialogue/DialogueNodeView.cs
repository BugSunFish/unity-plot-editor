﻿using Assets.Scripts.ScenarioSystem.Nodes.Option;
using Assets.unity_plot_editor.Nodes.Abstractions;
using Assets.unity_plot_editor.Nodes.Abstractions.Ports;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes.Dialogue
{
    public class DialogueNodeView : PlotNodeView, INormalNodeView, ILogicNodeView
    {
        public DialogueNode Node { get => node as DialogueNode; }
        public HashSet<OptionNodeView> OptionViews { get; set; }

        public NormalPort InputNormal { get; set; }
        public NormalPort OutputNormal { get; set; }

        public LogicPort InputLogic { get; set; }
        public LogicPort OutputLogic { get; set; }

        public TemplateContainer genInputContainer;
        public TemplateContainer genOutputContainer;

        public DialogueNodeView(DialogueNode dialogueNode)
        {
            node = dialogueNode;
            title = "Dialogue";
            viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            genInputContainer = new TemplateContainer();
            genOutputContainer = new TemplateContainer();

            OptionViews = new HashSet<OptionNodeView>();

            CreateInputNormalPort();
            CreateOutputNormalPort();

            CreateInputLogicPort();
            CreateOutputLogicPort();

            titleContainer.Insert(0, genInputContainer);
            titleContainer.Add(genOutputContainer);

            CreateButtonRepeat();

            CreateOptions();
        }

        public void CreateInputNormalPort()
        {
            InputNormal = NormalPort.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            InputNormal.portName = "";
            InputNormal.portColor = Color.white;

            genInputContainer.Add(InputNormal);
        }

        public void CreateOutputNormalPort()
        {
            OutputNormal = NormalPort.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            OutputNormal.portName = "";
            OutputNormal.portColor = Color.white;

            genOutputContainer.Add(OutputNormal);
        }

        public void CreateInputLogicPort()
        {
            InputLogic = LogicPort.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            InputLogic.portName = "";
            InputLogic.portColor = new Color(1, 0.5f, 0);

            genInputContainer.Add(InputLogic);
        }

        public void CreateOutputLogicPort()
        {
            OutputLogic = LogicPort.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));


            OutputLogic.portName = "";
            OutputLogic.portColor = new Color(1, 0.5f, 0);

            genOutputContainer.Add(OutputLogic);
        }

        public void CreateButtonRepeat()
        {
            var button = new Button();

            button.text = "↺";
            button.style.fontSize = 14;
            button.style.backgroundColor = Node.IsRepeats ? new Color(0.3f, 0.5f, 0.3f) : new Color(0.3f, 0.3f, 0.3f);
            button.clicked += () =>
            {
                Node.IsRepeats = !Node.IsRepeats;
                button.style.backgroundColor = Node.IsRepeats ? new Color(0.3f, 0.5f, 0.3f) : new Color(0.3f, 0.3f, 0.3f);
            };

            titleButtonContainer.Add(button);
        }

        public void CreateOptions()
        {
            foreach (var option in Node.Options)
            {
                foreach (var childOption in option.ChildNodes)
                {
                    if (childOption is OptionNode)
                    {
                        var optionView = new OptionNodeView(childOption as OptionNode);
                        contentContainer.Add(optionView.container);
                        OptionViews.Add(optionView);
                    }
                }
            }
        }

        public Port GetPortByGuid(Guid guid)
        {

            if (InputNormal.Guid == guid)
                return InputNormal;

            if (OutputNormal.Guid == guid)
                return OutputNormal;

            foreach (var optionView in OptionViews)
            {
                if (optionView.GetPortByGuid(guid) is Port port)
                    return port;
            }

            return null;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            evt.menu.AppendAction($"Info", (a) =>
            {
                Debug.Log("" +
                    $"Child Nodes Count:\t{Node.ChildNodes.Count}\n" +
                    $"Authors Count:\t{Node.Authors.Count}\n" +
                    $"IsRepeat:\t{Node.IsRepeats}\n" +
                    $"Logic:\t{(Node.LogicNode != null ? "Connnect" : " ")}\n" +
                    "");
            });
            evt.menu.AppendAction($"Add Test Options Structure", (a) =>
            {
                OptionNode optionStart = new OptionNode("N1", null, "Hello!");
                OptionNode option0 = new OptionNode("P1", "Hello", "Hello!");
                OptionNode option1 = new OptionNode("P1", "Bye", "Bye!");

                Node.StartNode = optionStart;
                Node.AddMessage(optionStart, option0);
                Node.AddMessage(optionStart, option1);

            });
        }
    }
}
