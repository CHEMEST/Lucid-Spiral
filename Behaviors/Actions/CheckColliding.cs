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
    /// This action checks for collisions between the source's Hitbox and other objects' hitboxes.
    /// </summary>
    [GlobalClass]
    internal partial class CheckColliding : ActionPattern
    {
        public override void Action(double delta)
        {
            Utils.ProcessCollisions(
                Source, CollisionType.Hitbox, CollisionType.Hitbox,
                (collision) =>
                {

                    GD.Print("Detected : " + collision.GetOwner().Name);

                    Speed status = Utils.FindStatus<Speed>(collision.GetOwner());
                    GD.Print(collision.GetOwner().Name + " " + status);
                });
        }
    }
}
