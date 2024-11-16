using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts.ScenarioSystem.Nodes.Dialogue
{
    public abstract class PlotNodeView : Node
    {
        public PlotNode node;
        public Port Input { get; set; }
        public Port Output { get; set; }
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
        }
    }
}
