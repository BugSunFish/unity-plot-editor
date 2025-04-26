using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

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
            return Create<Edge>(portOrientation, portDirection, portCapacity, type);
        }

        public static new LogicPort Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            var port = new LogicPort(orientation, direction, capacity, type);
            port.m_EdgeConnector = new EdgeConnector<TEdge>(new EdgeConnectorListener());
            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }
    }
}
