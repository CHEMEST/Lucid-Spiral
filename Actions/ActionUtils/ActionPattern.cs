using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LucidSpiral.Actions.ActionUtils
{
    internal abstract partial class ActionPattern : Node
    {
        [Export] public CharacterBody2D Source { get; private set; }
        /// <summary>
        /// Values less than 1 mean no max
        /// </summary>
        [Export] public int RepeatMax { get; private set; } = 1;
        [Export] public double RepeatDelayS { get; private set; } = 0;

        private int repeated = 0;
        private Timer delayTimer;

        public abstract void Act();

        public override void _Ready()
        {
            Debug.Assert(Source != null, "Action missing a CharacterBody2D Body to Act upon");

            delayTimer = new Timer
            {
                WaitTime = RepeatDelayS,
                Autostart = true,
                OneShot = false
            };

            AddChild(delayTimer);
            delayTimer.Timeout += OnDelayTimerTimeout;
        }

        private void OnDelayTimerTimeout()
        {
            if (repeated < RepeatMax)
            {
                Act();
                if (RepeatMax > 0) repeated++;
            }
            else
            {
                QueueFree();
            }
        }
    }
}
