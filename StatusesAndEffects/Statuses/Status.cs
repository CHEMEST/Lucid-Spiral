using Godot;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
/// <summary>
/// Holds a value of type T and a Modify function
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class Status : Node
{
    public object Value { get; private set; }

    public Status(object value)
    {
        Value = value;
    }

    public void Modify(Func<object, object> modifier) => Value = modifier.Invoke(Value);
}