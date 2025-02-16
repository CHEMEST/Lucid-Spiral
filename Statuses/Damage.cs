using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    [GlobalClass]
    internal partial class Damage : Status<double>
    {
        [Export] public new double Value = 10;
        public Damage(double baseValue) : base(baseValue) {

        }
        public Damage() : base(10)
        {
            
        }
    }
}
