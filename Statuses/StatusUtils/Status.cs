using Godot;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
/// <summary>
/// Holds a value of type Variant and a Modify function
/// This is one of the very very few use cases where using a Variant is ok
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class Status : Node, IStatus
{
    [Export] public Variant Value { get; private set; }
    public Status(Variant value) => Value = value;

    public void Modify(Func<Variant, Variant> modifier) => Value = modifier.Invoke(Value);
    public override string ToString() => $"{GetType().Name}: ({Value})";
}