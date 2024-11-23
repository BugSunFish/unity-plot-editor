using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using static UnityEditor.Experimental.GraphView.Port;

namespace Assets.unity_plot_editor.Nodes.Abstractions.Ports
{
    public class NormalPort : Port
    {
        public Guid Guid { get; set; }

        public NormalPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            name = typeof(NormalPort).Name;
            Guid = Guid.NewGuid();
        }

        public static NormalPort InstantiatePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
        {
            return new NormalPort(portOrientation, portDirection, portCapacity, type);
        }
    }
}
