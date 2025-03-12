using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Globals;
using LucidSpiral.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions
{
    [GlobalClass]
    internal partial class BounceAwayNearby : ActionPattern
    {
        public override void Action(double delta)
        {
            Utils.ProcessCollisions(
                Source, CollisionType.Hitbox, CollisionType.Hitbox,
                (collision) =>
                {
                    CharacterBody2D source = collision.Source();
                       // apply an effect lmao
                });
        }
    }
}
