using Godot;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
/// <summary>
/// Holds a double and a Modify function
/// I've fought too hard with this in attempt to make it generic and work with Godot, just make other classes for other types dude
/// </summary>
/// <typeparam></typeparam>
public partial class StatusD : Node, IStatus
{
    [Signal] public delegate void StatusChangedEventHandler(double value);

    [Export] public double Value { get; protected set; } = 0;
    /// <summary>
    /// If Max == -1, Max = inital Value
    /// </summary>
    [Export] public double Max { get; private set; } = Double.MaxValue;

    public override void _Ready()
    {
        base._Ready();
        //if (Max == -1) Max = Value;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        //if (Value > Max) Value = Max;
    }
    public StatusD() { }
    public StatusD(double value, double maxValue = -1)
    {
        Value = value;
        Max = maxValue;
    }

    public void Modify(Func<double, double> modifier)
    {
        modifier.Invoke(Value);
        EmitSignal(SignalName.StatusChanged, Value);
    }
    public override string ToString() => $"{GetType().Name}: ({Value})";
}