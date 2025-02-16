using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LucidSpiral.MovementPatterns.MovementPatternThings
{
    internal abstract partial class MovementPattern : Node
    {
        [Export] public CharacterBody2D Body { get; private set; }
        public abstract void Move();
        public override void _Ready()
        {
            Debug.Assert(Body != null, "MovementPattern missing a CharacterBody2D Body to Move");
        }
        public override void _PhysicsProcess(double delta)
        {
            Move();
        }
    }
}
