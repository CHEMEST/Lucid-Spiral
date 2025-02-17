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
        public Health(double value) : base(value)
        {
        }
        public Health() : base(0) { }
    }
}
