using Assets.Scripts.ScenarioSystem.Nodes.Dialogue;
using Assets.unity_plot_editor.Nodes.Abstractions;
using Assets.unity_plot_editor.Nodes.Abstractions.Ports;
using System;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.ScenarioSystem.Nodes
{
    public interface ILogicNodeView
    {
        public LogicPort InputLogic { get; set; }
        public LogicPort OutputLogic { get; set; }

        public void CreateInputLogicPort();
        public void CreateOutputLogicPort();
        public Port GetPortByGuid(Guid guid);
        public ILogicNode GetLogicNodeByPortGuid(Guid guid);
        public ILogicNode GetLogicNodeByNodeGuid(string guid);
    }
}
