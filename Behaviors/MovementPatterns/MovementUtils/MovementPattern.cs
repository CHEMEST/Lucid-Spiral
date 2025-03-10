using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using LucidSpiral.Behaviors.MovementPatterns.MovementUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LucidSpiral.MovementPatterns.MovementPatternThings
{
    internal abstract partial class MovementPattern : Node2D, IMovement
    {
        public bool CanMove { get; set; } = true;
        [Export] public CharacterBody2D Body { get; private set; }

        public void Act(double delta)
        {
            if (!CanMove) return;
            Move(delta);
        }

        public abstract void Move(double delta);

        public override void _Ready()
        {
            if (Body == null)
            {
                Node owner = GetOwner();
                if (owner is CharacterBody2D)
                {
                    Body = owner as CharacterBody2D;
                }
            }
            Debug.Assert(Body != null, "MovementPattern missing a CharacterBody2D Body to Move");
        }
    }
}
