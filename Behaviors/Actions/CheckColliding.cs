using Godot;
using LucidSpiral.Actions.ActionUtils;
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
    [GlobalClass]
    internal partial class CheckColliding : ActionPattern
    {
        public override void Action()
        {
            CollisionManager collisionManager = Source.GetNodeOrNull<ManagerHub>("ManagerHub").GetManager<CollisionManager>();
            CollisionSet collisionSet = collisionManager.GetActiveBehavior() as CollisionSet;

            Godot.Collections.Array<Area2D> overlappingAreas = collisionSet.Area.GetOverlappingAreas();
            foreach (Area2D area in overlappingAreas)
            {
                GD.Print("Detected");
                ManagerHub hub = area.GetTree().Root.GetNodeOrNull<ManagerHub>("ManagerHub");
                if (hub == null) {
                    GD.Print("Error?");
                    continue; }

                Speed status = hub.GetManager<StatusManager>().GetStatus<Speed>();
                status.Modify(x => x * 2);
                GD.Print(status.Value);

            }
        }
    }
}
