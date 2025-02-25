using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.BehaviorUtils
{
    internal interface IBehavior
    {
        public abstract void Act(double delta);
    }
}
