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
        public override void Action()
        {
            CollisionManager collisionManager = Source.GetNodeOrNull<ManagerHub>("ManagerHub").GetManager<CollisionManager>();
            CollisionSet collisionSet = collisionManager.GetCollisionSet(CollisionType.Hitbox);
            if (collisionSet == null) { throw new NotSupportedException(); }

            List<CollisionSet> collisions = collisionSet.GetOverlappingCollisionSets(CollisionType.Hitbox);
            foreach (CollisionSet collision in collisions)
            {
                GD.Print("Detected : " + collision);
                Speed status = Utils.FindManager<StatusManager>(collision).GetStatus<Speed>();
                GD.Print(collision.GetOwner().Name + " " + status);

            }
        }
    }
}
