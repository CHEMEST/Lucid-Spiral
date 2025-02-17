using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Statuses
{
    [GlobalClass]
    internal partial class Speed : Status
    {
        public Speed(Variant value) : base(value)
        {
        }
        public Speed() : base(0)
        {

        }
    }
}
