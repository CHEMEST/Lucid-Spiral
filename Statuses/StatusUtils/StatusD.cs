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
    [Export] public double Value { get; private set; } = 0;
    public StatusD(double value) => Value = value;

    public void Modify(Func<double, double> modifier) => Value = modifier.Invoke(Value);
    public override string ToString() => $"{GetType().Name}: ({Value})";
}