using Godot;
using LucidSpiral.Behaviors.Effects.EffectUtils;
using LucidSpiral.Globals;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Effects
{
    internal partial class FireDamage : Effect
    {
        private double _dmg;
        public FireDamage(double lifespanS, double delayS, double dmg, bool auto = true) : base(lifespanS, delayS, auto)
        {
            _dmg = dmg;
        }

        public override void Affect()
        {
            Utils.FindStatus<Health>(Utils.FindEntityCarrying(this)).Modify(x => x -= _dmg);
        }
    }
}
