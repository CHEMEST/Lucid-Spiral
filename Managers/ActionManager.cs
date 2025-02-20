using Godot;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LucidSpiral.MovementPatterns;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using System.Diagnostics;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Managers.ManagerUtils;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class ActionManager : BehaviorManager<ActionPattern>
    {
        public ActionManager() { }
        public override void _Ready()
        {
            base._Ready();
            GD.Print(Behaviors.ToString());

        }

    }
}
