using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LucidSpiral.MovementPatterns.MovementPatternThings
{
    internal abstract partial class MovementPattern : Node, IBehavior
    {
        [Export] public CharacterBody2D Body { get; private set; }
        public abstract void Move();
        public void Act() { Move(); }
        public override void _Ready()
        {
            Debug.Assert(Body != null, "MovementPattern missing a CharacterBody2D Body to Move");
        }
    }
}
