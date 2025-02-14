using Godot;
using System;

public class StatusEffect<T>
{
    public string Name { get; private set; }
    public Func<T, T> Modifier { get; private set; }
    public Status<T> TargetStatus { get; private set; }
    private Ticker LifeSpan;

    public StatusEffect(string name, Status<T> target, Func<T, T> modifier, float duration)
    {
        Name = name;
        TargetStatus = target;
        Modifier = modifier;
        LifeSpan = new Ticker(duration, 0.1f);
    }

    public void Apply()
    {
        TargetStatus.AddModifier(Modifier);
    }

    public void Remove()
    {
        TargetStatus.RemoveModifier(Modifier);
    }

    public bool Update(float delta)
    {
        if (LifeSpan.Update(delta))
        {
            Remove();
            return true;
        }
        return false;
    }
}