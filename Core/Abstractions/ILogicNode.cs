using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.ScenarioSystem.Nodes
{
    public interface ILogicNode
    {
        public bool FromData { get; set; }
        public ILogicNode LogicNode { get; set; }
        public void SetLogic(ILogicNode logicNode);
        public void ClearLogic();
        public bool GetLogic();
    }
}
