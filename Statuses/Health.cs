using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    [GlobalClass]
    internal partial class Health : StatusD
    {
        [Signal] public delegate void HealthDepletedEventHandler();
        public Health() : base(0){ }
        public new void Modify(Func<double, double> modifer)
        {
            base.Modify(modifer);
            if (Value < 0)
            {
                EmitSignal(SignalName.HealthDepleted);
            }
        }
        public Health(double value) : base(value) { }
    }
}
