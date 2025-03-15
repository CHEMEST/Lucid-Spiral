using Godot;
using LucidSpiral.Behaviors.Effects.EffectUtils;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Managers.ManagerUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class EffectManager : Node2D, IManager
    {
        public override void _Ready()
        {
            base._Ready();
            ManagerHub hub = GetParent() as ManagerHub;
            hub.AddManager(this);
        }
        public override string ToString()
        {
            string log = "Active Effects \n {";

            foreach (Node child in GetChildren())
            {
                log += child.GetType().ToString() + " ";
            }
            log += "\n}";
            return log;
        }
    }
}
