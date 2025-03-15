using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Behaviors.Effects.EffectUtils;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions.InflictAndDie
{
    internal abstract partial class InflictEffectOnImpactAndDie : ActionPattern
    {
        public override void Action(double delta)
        {
            Utils.ProcessCollisions(
                Source, CollisionType.Hitbox, CollisionType.Hitbox,
                (collision) =>
                {
                    Effect effect = CreateEffect();
                    Utils.ApplyEffect(collision.GetOwner(), effect);
                    Source.QueueFree();
                });
        }

        public abstract Effect CreateEffect();
    }
}
