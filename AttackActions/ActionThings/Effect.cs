using Godot;
using System;

public abstract class Effect
{
    protected Creature Target;
    private Ticker LifeSpan;

    public Effect(Creature target, float duration, float tickSpeed)
    {
        Target = target;
        LifeSpan = new Ticker(duration, tickSpeed);
    }

    public bool Update(float delta)
    {
        ApplyEffect();
        return LifeSpan.Update(delta);
    }

    protected abstract void ApplyEffect();
}