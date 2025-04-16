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
        private double dmg;
        public FireDamage(double lifespanS, double delayS, double dmg, bool auto = true) : base(lifespanS, delayS, auto)
        {
            this.dmg = dmg;
        }

        public override void Affect()
        {
            Entities.Creatures.Entity root = Utils.FindEntityCarrying(this);

            Health health = Utils.FindStatus<Health>(root);
            health?.Modify(x => x -= dmg);
        }
    }
}
