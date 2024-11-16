using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.ScenarioSystem.Nodes.Dialogue
{
    public class DialogueNodeView : PlotNodeView, ILogicNodeView
    {
        public ILogicNode AsLogicNode { get => node as ILogicNode; }
        public DialogueNode Node { get => node as DialogueNode; }

        public Port InputLogic { get; set; }
        public Port OutputLogic { get; set; }


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

            CreateInputPorts();
            CreateOutputPorts();

            CreateInputLogicPort();
            CreateOutputLogicPort();

            titleContainer.Insert(0, genInputContainer);
            titleContainer.Add(genOutputContainer);

            CreateButtonRepeat();

            CreateOptions();
        }

        public void CreateInputPorts()
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            Input.name = "NormalPort";
            Input.portName = "";
            Input.portColor = Color.white;

            genInputContainer.Add(Input);
        }

        public void CreateOutputPorts()
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            Output.name = "NormalPort";
            Output.portName = "";
            Output.portColor = Color.white;

            genOutputContainer.Add(Output);
        }

        public void CreateInputLogicPort()
        {
            InputLogic = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            InputLogic.name = "LogicPort";
            InputLogic.portName = "";
            InputLogic.portColor = new Color(1, 0.5f, 0);

            genInputContainer.Add(InputLogic);
        }

        public void CreateOutputLogicPort()
        {
            OutputLogic = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            OutputLogic.name = "LogicPort";
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
                contentContainer.Add(new Option.OptionNodeView(option).container);
            }
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
        }

    }
}
