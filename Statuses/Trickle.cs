using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Statuses
{
    [GlobalClass]
    internal partial class Trickle : StatusD
    {
        public Trickle() : base(0) { }
        public Trickle(double value) : base(value)
        {
        }
    }
}
