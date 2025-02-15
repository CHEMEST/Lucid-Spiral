using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    [GlobalClass]
    internal partial class Health : Status
    {
        [Export] public new double Value = 100;
        public Health(double baseValue) : base(baseValue) {

        }
        public Health() : base(100)
        {
            
        }
    }
}
