using Godot;
using LucidSpiral.Behaviors.BehaviorUtils;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers.ManagerUtils
{
    internal abstract partial class UnfocusedManager<T> : Node, IManager where T : class, IBehavior
    {
        public List<T> Behaviors { get; private set; } = new();
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
            base._Process(delta);
            foreach (T behavior in Behaviors)
            {
                behavior.Act();
            }
        }
    }
}
