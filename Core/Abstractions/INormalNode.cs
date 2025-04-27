using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.unity_plot_editor.Nodes.Abstractions
{
    public interface INormalNode
    {
        public List<INormalNode> ChildNodes { get; set; }
        public void AddChild(INormalNode node);
        public void RemoveChild(INormalNode node);
    }
}
