using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Creature : CharacterBody2D
{
    public Dictionary<string, object> Statuses = new();
    private List<StatusEffect<object>> ActiveEffects = new();

    public void AddStatus<T>(string name, T baseValue)
    {
        Statuses[name] = new Status<T>(name, baseValue);
    }

    public Status<T> GetStatus<T>(string name)
    {
        return Statuses[name] as Status<T>;
    }

    public void ApplyStatusEffect<T>(StatusEffect<T> effect)
    {
        ActiveEffects.Add(effect as StatusEffect<object>);
    }

    public override void _Process(double delta)
    {
        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            GD.Print("Status " + i + ":" + ActiveEffects[i]);
            GD.Print("Active Effect " + i + ":" + ActiveEffects[i]);
            if (ActiveEffects[i].Update((float)delta))
                ActiveEffects.RemoveAt(i);
        }
    }
}
