using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlotNode : ScriptableObject
{
    public string guid;
    public Vector2 position;

    public abstract List<PlotNode> ChildNodes { get; set; }

    public abstract void AddChild(PlotNode node);
    public abstract void RemoveChild(PlotNode node);

}
