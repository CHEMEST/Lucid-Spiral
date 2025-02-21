using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using System;
using System.Diagnostics;
/// <summary>
/// CollisionSet is currently just a wrapper for Area2D but this allows for integration with the ManagerHub system and future implementations of multiple Areas
/// </summary>
[GlobalClass]
public partial class CollisionSet : Node, IBehavior
{
    /// <summary>
    /// use GetOverlappingAreas()
    /// optimize?
    /// </summary>
    [Export] public Area2D Area { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        Debug.Assert(Area != null, "Area not assigned");
    }
    public void Act()
    {

    }
}
