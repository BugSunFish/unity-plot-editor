using Assets.unity_plot_editor.Nodes.Abstractions.Ports;
using System;
using UnityEditor.Experimental.GraphView;

namespace Assets.unity_plot_editor.Nodes.Abstractions
{
    public interface INormalNodeView
    {
        public NormalPort InputNormal { get; set; }
        public NormalPort OutputNormal { get; set; }

        public void CreateInputNormalPort();
        public void CreateOutputNormalPort();
        public Port GetPortByGuid(Guid guid);
    }
}
