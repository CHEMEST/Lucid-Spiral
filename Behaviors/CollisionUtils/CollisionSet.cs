using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using System;

public partial class CollisionSet : Node, IBehavior
{
    /// <summary>
    /// use GetOverlappingAreas()
    /// optimize?
    /// </summary>
    [Export] public Area2D Area { get; private set; }
    public void Act()
    {
        throw new NotImplementedException();
    }
}
