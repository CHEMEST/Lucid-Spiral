using Godot;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
/// <summary>
/// Holds a value of type T and a Modify function
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class Status<T> : Node, IStatus
{
    public T Value { get; private set; }

    public Status(T value)
    {
        Value = value;
    }

    public void Modify(Func<T, T> modifier) => Value = modifier.Invoke(Value);
    public override string ToString()
    {
        return $"{GetType().Name}: ({Value})";
    }
}