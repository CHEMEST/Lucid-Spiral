using Godot;
using LucidSpiral.Actions.ActionUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Actions
{
    [GlobalClass]
    internal partial class EmptyAction : ActionPattern

    {
        public override void Act()
        {
            GD.Print("Acting!");
        }
    }
}
