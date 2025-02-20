using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.BehaviorUtils;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers.ManagerUtils
{
    internal abstract partial class BehaviorManager : Node, IManager
    {
        public List<IBehavior> Behaviors { get; private set; } = new();
        public int ActiveIndex { get; private set; } = 0;
        public BehaviorManager() { }

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is IBehavior)
                {
                    Behaviors.Add(child as IBehavior);
                }
            }
        }
        public override void _Process(double delta)
        {
            if (ActiveIndex >= Behaviors.Count) ActiveIndex = 0;
            Behaviors[ActiveIndex].Act();
        }
        public void NextBehaviorPattern()
        {
            if (ActiveIndex < Behaviors.Count)
            {
                ActiveIndex++;
            }
            else
            {
                ActiveIndex = 0;
            }
        }
        public void ResetBehaviorCycle()
        {
            ActiveIndex = 0;
        }
    }
}
