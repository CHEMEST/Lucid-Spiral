using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using LucidSpiral.Behaviors.MovementPatterns.MovementUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LucidSpiral.MovementPatterns.MovementPatternThings
{
    internal abstract partial class MovementPattern : Node, IMovement
    {
        public bool CanMove { get; set; } = true;
        [Export] public CharacterBody2D Body { get; private set; }

        public void Act(double delta)
        {
            Move(delta);
        }

        public abstract void Move(double delta);

        public override void _Ready()
        {
            Debug.Assert(Body != null, "MovementPattern missing a CharacterBody2D Body to Move");
        }
    }
}
