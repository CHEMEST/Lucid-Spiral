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
    internal abstract partial class BehaviorManager<T> : Node, IManager where T : class, IBehavior
    {
        public List<T> Behaviors { get; private set; } = new();
        public T ActiveBehavior { get; set; }
        public int ActiveIndex { get; private set; } = 0;
        public BehaviorManager() { }

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is IBehavior)
                {
                    Behaviors.Add(child as T);
                }
            }
            Debug.Assert(Behaviors.Count > 0);
            ActiveBehavior = Behaviors[0];
        }
        public override void _Process(double delta)
        {
            ActiveBehavior.Act();
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
            ActiveBehavior = Behaviors[ActiveIndex];
        }
        public void ResetBehaviorCycle()
        {
            ActiveIndex = 0;
            ActiveBehavior = Behaviors[ActiveIndex];
        }
    }
}
