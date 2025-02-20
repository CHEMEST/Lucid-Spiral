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
        [Export] public double RepeatDelayS { get; private set; }
        private bool _ready = false;

        private int _repeated = 0;
        private Timer _delayTimer;

        public void Act()
        {
            if (_ready)
            {
                Behavior.Act();
                _ready = false;
            }
        }

        public override void _Ready()
        {
            Behavior = (IBehavior) GetChild(0, true);
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
            _ready = true;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        private void OnDelayTimerTimeout()
        {
            if (_repeated < RepeatMax)
            {
                if (!_ready)
                {
                    if (RepeatMax > 0) _repeated++;
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
