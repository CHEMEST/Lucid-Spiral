using Godot;
using System;
using System.Collections.Generic;

public class AttackAction
{
    public Creature Source;
    public List<CollisionShape> CollisionShapes = new List<CollisionShape>();
    public List<MovementPattern> MovementPatterns = new List<MovementPattern>();
    public List<Ticker> MovementTickers = new List<Ticker>();
    public List<Effect> Effects = new List<Effect>();


    private int CurrentIndex = 0;

    public CollisionShape CurrentCollision => CollisionShapes[CurrentIndex];
    public MovementPattern CurrentMovement => MovementPatterns[CurrentIndex];
    public Ticker CurrentTicker => MovementTickers[CurrentIndex];

    public void Update(float delta)
    {
        if (CurrentIndex >= MovementTickers.Count) return;

        CurrentMovement.Update(this, delta);
        CurrentCollision.CheckCollision(this);

        foreach (var effect in Effects)
        {
            if (effect.Update(delta))
            {
                Effects.Remove(effect);
            }
        }

        if (CurrentTicker.Update(delta))
        {
            CurrentIndex++;
        }
    }
}
