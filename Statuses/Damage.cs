using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    [GlobalClass]
    internal partial class Damage : StatusD
    {
        public Damage(double value) : base(value)
        {
        }
        public Damage() : base(0) { }
    }
}
