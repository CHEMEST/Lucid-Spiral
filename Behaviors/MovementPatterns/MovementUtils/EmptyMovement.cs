using Godot;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.MovementPatterns
{
    [GlobalClass]
    internal partial class EmptyMovement : MovementPattern
    {
        public override void Move(double delta)
        {
            GD.Print("No Movement");
        }
        public override void _Process(double delta)
        {
            Move(delta);
        }
    }
}
