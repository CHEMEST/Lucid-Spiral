using Godot;
using System;

public abstract class MovementPattern
{
    public abstract void Update(AttackAction action, float delta);
}