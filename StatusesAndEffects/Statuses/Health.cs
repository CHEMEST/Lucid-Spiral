using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    internal class Health : Status<double>
    {
        public Health(double baseValue) : base(baseValue) { }
    }
}
