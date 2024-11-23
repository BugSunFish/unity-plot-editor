using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.unity_plot_editor.Nodes.Abstractions.Ports
{
    public class LogicPort : Port
    {
        public Guid Guid { get; set; }

        public LogicPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            name = typeof(LogicPort).Name;
            Guid = Guid.NewGuid();
        }

        public static LogicPort InstantiatePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
        {
            return new LogicPort(portOrientation, portDirection, portCapacity, type);
        }
    }
}
