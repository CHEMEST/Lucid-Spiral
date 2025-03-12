using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// stores an Area, a Type, and some helper functions
/// </summary>
[GlobalClass]
public partial class CollisionSet : Node2D
{
    [Export] public CollisionType Type { get; private set; } = CollisionType.Empty;
    public List<Node> Ignoring { get; private set; } = new();
    public Area2D Area { get; private set; }
    public bool IsDetectable { get; set; } = true;
    public CharacterBody2D Source() { return GetOwner() as CharacterBody2D; }
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
                if (Ignoring.Contains(collisionSet.Source())) continue;
                if (collisionSet.Type == collisionType && collisionSet.IsDetectable)
                {
                    collisionSets.Add(collisionSet);
                }
            }
        }
        return collisionSets;
    }

}
