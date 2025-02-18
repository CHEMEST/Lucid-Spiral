using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.BehaviorUtils
{
    internal abstract partial class BehaviorSet<T> : Node, IBehavior where T : class, IBehavior
    {
        [Export] public CharacterBody2D Source { get; private set; }
        /// <summary>
        /// Values less than 1 means no max
        /// </summary>
        //// Values less than 1 means no max
        [Export] public int RepeatMax { get; private set; } = 1;
        public T Behavior { get; private set; }
        [Export] public double RepeatDelayS { get; private set; } = 0;

        private int repeated = 0;
        private Timer delayTimer;

        public void Act()
        {
            Behavior.Act();
        }

        public override void _Ready()
        {
            Debug.Assert(Source != null, "Action missing a CharacterBody2D Body to Act upon");
            if (RepeatDelayS > 0)
            {
                delayTimer = new Timer
                {
                    WaitTime = RepeatDelayS,
                    Autostart = true,
                    OneShot = false
                };

                AddChild(delayTimer);
                delayTimer.Timeout += OnDelayTimerTimeout;
            }
            else // instant all
            {
                for (int i = 0; i < RepeatMax; i++)
                {
                    Act();
                }
            }
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
