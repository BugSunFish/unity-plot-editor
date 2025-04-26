using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.Port;
using UnityEngine;

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
            return Create<Edge>(portOrientation, portDirection, portCapacity, type);
        }

        public static new NormalPort Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            var port = new NormalPort(orientation, direction, capacity, type);
            port.m_EdgeConnector = new EdgeConnector<TEdge>(new EdgeConnectorListener());
            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }
    }
}
