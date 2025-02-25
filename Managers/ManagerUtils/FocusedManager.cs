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
    /// <summary>
    /// Not currently being used by anything. Same core structure as MovementManager 
    /// but that needs to run off of Physics Process 
    /// and needs a param that IBehavior.Act() does not have
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract partial class FocusedManager<T> : Node, IManager where T : class, IBehavior
    {
        public List<T> Behaviors { get; private set; } = new();
        public int ActiveIndex { get; private set; } = 0;
        public T GetActiveBehavior() { return Behaviors[ActiveIndex]; }

        public FocusedManager() { }

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is T)
                {
                    Behaviors.Add(child as T);
                }
            }
        }
        public override void _Process(double delta)
        {
            if (ActiveIndex >= Behaviors.Count) ActiveIndex = 0;
            Behaviors[ActiveIndex].Act(delta);
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
