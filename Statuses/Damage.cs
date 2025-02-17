using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.StatusesAndEffects.Statuses
{
    [GlobalClass]
    internal partial class Damage : Status
    {
        public Damage(Variant value) : base(value)
        {
        }
        public Damage() : base(0) { }
    }
}
