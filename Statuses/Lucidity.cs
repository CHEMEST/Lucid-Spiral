using Godot;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    [GlobalClass]
    internal partial class Lucidity : StatusD
    {
        [Signal] public delegate void LucidityDepletedEventHandler();
        [Export] private double intervalOfTrickleTrigger = 1;
        private Timer timer;
        public Lucidity() : base(0){ }
        public override void _Ready()
        {
            base._Ready();
            timer = new Timer
            {
                Autostart = true,
                WaitTime = intervalOfTrickleTrigger
            };
            timer.Timeout += TriggerTrickle;
            CallDeferred(nameof(StartNewTrickle));
        }
        private void TriggerTrickle()
        {
            Trickle trickle = Utils.FindManager<StatusManager>(GetOwner()).GetStatus<Trickle>();
            Modify((v) => v - trickle.Value);
        }
        private void StartNewTrickle()
        {
            timer.Start();
        }
        public new void Modify(Func<double, double> modifer)
        {
            base.Modify(modifer);
            if (Value < 0)
            {
                EmitSignal(SignalName.LucidityDepleted);
            }
        }

        public Lucidity(double value) : base(value) { }
    }
}
