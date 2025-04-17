using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Statuses;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions
{
    /// <summary>
    /// This action checks for collisions between the source's Secondary Hitbox and other objects' hitboxes.
    /// </summary>
    [GlobalClass]
    internal partial class AoEHurt : ActionPattern
    {
        [Export] private float dmgMult = 3f;
        public override void Action(double delta)
        {
            Utils.ProcessCollisions(
                Source, CollisionType.SecondaryHitbox, CollisionType.Hitbox,
                (collision) =>
                {
                    Health health = Utils.FindStatus<Health>(collision.GetOwner());
                    Damage damage = Utils.FindStatus<Damage>(Source);

                    if (damage == null) { GD.PrintErr("Sword has no damage"); }
                    health?.Modify((v) => v-= damage.Value*dmgMult);
                    GD.Print("Hit : ",  collision.GetOwner().Name, " | " , health);
                });
        }
    }
}
