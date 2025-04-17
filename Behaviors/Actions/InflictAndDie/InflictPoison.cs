using Godot;
using LucidSpiral.Behaviors.Effects;
using LucidSpiral.Behaviors.Effects.EffectUtils;
using LucidSpiral.Globals;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions.InflictAndDie
{
    [GlobalClass]
    internal partial class InflictPoison : InflictEffectOnImpactAndDie
    {
        [Export] private double _fireLifespan = 5;
        [Export] private double _dmgPerTick = 5;

        public override Effect CreateEffect()
        {
            return new Poison(_fireLifespan, 0.5, _dmgPerTick);
        }
    }
}
