using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LucidSpiral.Actions.ActionUtils
{
    internal abstract partial class ActionPattern : Node, IBehavior
    {
        [Export] public CharacterBody2D Source { get; private set; }
        /// <summary>
        /// Values less than 1 means no max
        /// </summary>
        //// Values less than 1 means no max
        [Export] public int Repeats { get; private set; } = 1;

        public void Act()
        {
            for (int i = 0; i < Repeats; i++)
            {
                Action();
            }
            
        }
        public abstract void Action();

        public override void _Ready()
        {
            Debug.Assert(Source != null, "Action missing a CharacterBody2D Body to Act upon");
        }
    }
}
