using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// CollisionSet is currently just a wrapper for Area2D but this allows for integration with the ManagerHub system and future implementations of multiple Areas
/// </summary>
[GlobalClass]
public partial class CollisionSet : Node2D, IBehavior
{

    [Export] public CollisionType Type { get; private set; }
    public Area2D Area { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        Area = GetChild<Area2D>(0, true);
        Debug.Assert(Area != null, "Assign Area2D to " + GetOwner() + "'s " + this.GetType().Name);
    }

    public List<CollisionSet> GetOverlappingCollisionSets(CollisionType collisionType)
    {
        List<CollisionSet> collisionSets = new();
        foreach (Area2D area in Area.GetOverlappingAreas())
        {
            if (area.GetParent() is CollisionSet)
            {
                CollisionSet collisionSet = area.GetParent() as CollisionSet;
                if (collisionSet.Type == collisionType)
                {
                    collisionSets.Add(collisionSet);
                }
            }
        }
        return collisionSets;
    }

    public void Act()
    {

    }
}
