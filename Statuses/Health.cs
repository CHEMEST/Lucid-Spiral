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
        public Health() { }
        private double past = 0;
        public override void _Process(double delta)
        {
            base._Process(delta);
            if (past != Value)
            {
                past = Value;
                GD.Print(this);
            }
        }
        public Health(double value) : base(value) { }
    }
}
