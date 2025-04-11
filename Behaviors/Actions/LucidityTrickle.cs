using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Statuses;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions
{
    [GlobalClass]
    internal partial class LucidityTrickle : ActionPattern
    {
        public override void Action(double delta)
        {
            Trickle trickle = Utils.FindManager<StatusManager>(GetOwner()).GetStatus<Trickle>();
            Health health = Utils.FindManager<StatusManager>(GetOwner()).GetStatus<Health>();

            health.Modify((v) => v - trickle.Value);
        }
    }
}
