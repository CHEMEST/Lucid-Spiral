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

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class ActionManager : Node, IManager
    {
        public List<ActionPattern> Actions { get; private set; } = new();
        public ActionPattern ActiveAction { get; set; }
        public int ActiveIndex { get; private set; } = 0;
        public ActionManager() { }

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is ActionPattern)
                    {
                    Actions.Add(child as ActionPattern);
                }
            }
            Debug.Assert(Actions.Count > 0);
            ActiveAction = Actions[0];
        }
        public void NextActionPattern()
        {
            if (ActiveIndex < Actions.Count)
            {
                ActiveIndex++;
            }
            else
            {
                ActiveIndex = 0;
            }
            ActiveAction = Actions[ActiveIndex];
        }
        public void ResetActionCycle()
        {
            ActiveIndex = 0;
            ActiveAction = Actions[ActiveIndex];
        }
    }
}
