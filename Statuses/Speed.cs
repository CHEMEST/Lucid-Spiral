using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Statuses
{
    [GlobalClass]
    internal partial class Speed : Status<float>
    {
        [Export] public new float Value = 300f;
        public Speed(float baseValue) : base(baseValue)
        {

        }
        public Speed() : base(300f)
        {

        }
    }
}
