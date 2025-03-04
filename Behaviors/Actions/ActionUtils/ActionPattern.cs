using Godot;
using LucidSpiral.Behaviors.Actions.ActionUtils;
using LucidSpiral.Behaviors.BehaviorUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LucidSpiral.Actions.ActionUtils
{
    internal abstract partial class ActionPattern : Node, IAction
    {
        [Export] public CharacterBody2D Source { get; private set; }
        [Export] public bool IsActive { get; set; } = true;
        //// amount of times the Action() is run per _Process()
        [Export] public int Repeats { get; private set; } = 1;

        public void Act(double delta)
        {
            if (!IsActive) return;
            for (int i = 0; i < Repeats; i++)
            {
                Action(delta);
            }
        }
        public abstract void Action(double delta);

        public override void _Ready()
        {
            Debug.Assert(Source != null, "Action missing a CharacterBody2D Body to Act upon");
        }
    }
}
