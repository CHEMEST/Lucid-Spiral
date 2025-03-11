using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Globals;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions
{
    [GlobalClass]
    internal partial class DamageAndDie : ActionPattern
    {
        public override void Action(double delta)
        {
            Utils.ProcessCollisions(
                Source, CollisionType.Hitbox, CollisionType.Hitbox,
                (collision) =>
                {
                    double damage = Utils.FindStatus<Damage>(Source).Value;
                    Utils.FindStatus<Health>(collision.GetOwner()).Modify(x => x - damage);
                });
        }
    }
}
