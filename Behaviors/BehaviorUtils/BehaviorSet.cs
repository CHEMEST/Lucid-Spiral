using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.BehaviorUtils
{
    internal abstract partial class BehaviorSet<T> : Node where T : class, IBehavior
    {
        /// <summary>
        /// Values less than 1 means no max
        /// </summary>
        //// Values less than 1 means no max
        [Export] public int RepeatMax { get; private set; } = 0;
        public T Behavior { get; private set; }
        /// <summary>
        /// Values of 0 or less mean no timed trigger, only triggers by external action
        /// </summary>
        //// Values of 0 or less mean no timed trigger, only triggers by external action
        [Export] public double RepeatDelayS { get; private set; } = 0;
        private bool ready { get; set; } = false;
        public bool Enabled { get; set; } = true;

        private int _repeated = 0;
        private Timer _delayTimer;
        public void Act(double delta) 
        {
            if (!ready || !Enabled) return;

            Behavior.Act(delta);
            ready = false;

        }

        public override void _Ready()
        {
            Node child = GetChild(0, true);
            if (child is T) Behavior = child as T;

            Debug.Assert(Behavior != null, "BehaviorSet missing IBehavior to Act upon");
            if (RepeatDelayS > 0)
            {
                _delayTimer = new Timer
                {
                    WaitTime = RepeatDelayS,
                    Autostart = true,
                    OneShot = false
                };

                AddChild(_delayTimer);
                _delayTimer.Timeout += OnDelayTimerTimeout;
            }
            else
            {
                // wait for external trigger
            }


        }
        public void Trigger()
        {
            ready = true;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        private void OnDelayTimerTimeout()
        {
            if (_repeated < RepeatMax || RepeatMax < 1)
            {
                if (ready) return;
                
                if (RepeatMax >= 1) _repeated++;
                ready = true;
            }
            else
            {
                QueueFree();
            }
        }
    }
}
