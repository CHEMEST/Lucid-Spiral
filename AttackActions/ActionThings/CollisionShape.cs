using Godot;
using System;

public abstract class CollisionShape
{
    public abstract void CheckCollision(AttackAction action);
}