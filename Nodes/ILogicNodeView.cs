using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.ScenarioSystem.Nodes
{
    public interface ILogicNodeView
    {
        public ILogicNode AsLogicNode { get; }
        public Port InputLogic { get; set; }
        public Port OutputLogic { get; set; }

        public void CreateInputLogicPort();
        public void CreateOutputLogicPort();
    }
}
