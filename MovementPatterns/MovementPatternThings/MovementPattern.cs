using Godot;
using System;
using System.Collections.Generic;

namespace LucidSpiral.MovementPatterns.MovementPatternThings
{
    internal abstract partial class MovementPattern : Node
    {
        [Export] public CharacterBody2D Body { get; private set; }
        public abstract void Move();

    }
}
