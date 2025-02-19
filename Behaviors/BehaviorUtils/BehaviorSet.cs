using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.BehaviorUtils
{
    [GlobalClass]
    internal partial class BehaviorSet : Node, IBehavior
    {
        /// <summary>
        /// Values less than 1 means no max
        /// </summary>
        //// Values less than 1 means no max
        [Export] public int RepeatMax { get; private set; } = 1;
        public IBehavior Behavior { get; private set; }
        [Export] public double RepeatDelayS { get; private set; } = 0;
        private bool _ready = false;

        private int repeated = 0;
        private Timer delayTimer;

        public void Act()
        {
            
        }

        public override void _Ready()
        {
            Behavior = (IBehavior) GetChild(0, true);
            Debug.Assert(Behavior != null, "Action missing a CharacterBody2D Body to Act upon");
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
                    Behavior.Act();
                }
            }
        }

        private void OnDelayTimerTimeout()
        {
            if (repeated < RepeatMax)
            {
                if (!_ready)
                {
                    if (RepeatMax > 0) repeated++;
                    _ready = true;
                }
            }
            else
            {
                QueueFree();
            }
        }
    }
}
