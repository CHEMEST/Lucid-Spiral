using Godot;
using System;
using System.Collections.Generic;

public class Status<T>
{
    public string Name { get; private set; }
    private T _baseValue;
    private List<Func<T, T>> _modifiers = new();

    public Status(string name, T baseValue)
    {
        Name = name;
        _baseValue = baseValue;
    }

    public void AddModifier(Func<T, T> modifier) => _modifiers.Add(modifier);
    public void RemoveModifier(Func<T, T> modifier) => _modifiers.Remove(modifier);

    public T GetValue()
    {
        T modifiedValue = _baseValue;
        foreach (var modifier in _modifiers)
        {
            modifiedValue = modifier(modifiedValue);
        }
        return modifiedValue;
    }
}