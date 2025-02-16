using Godot;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Managers;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LucidSpiral.Statuses;

namespace LucidSpiral.MovementPatterns
{
    [GlobalClass]
    internal partial class ChasePlayer : MovementPattern
    {
        public ChasePlayer() { }
        public override void Move()
        {
            //GD.Print(GetNode<ManagerHub>("../..").GetManager<StatusManager>().GetStatus<Speed>().Value);
            float Speed = GetNode<ManagerHub>("../..").GetManager<StatusManager>().GetStatus<Speed>().Value;
            CharacterBody2D Player = Global.Player;

            Vector2 velocityTemp = Body.Velocity;

            Vector2 direction = (Player.Position - Body.Position).Normalized();
            float stopDistance = 10.0f;
            if (Body.Position.DistanceTo(Player.Position) > stopDistance)
            {
                velocityTemp = direction * Speed;
            }
            else
            {
                velocityTemp.X = Mathf.MoveToward(Body.Velocity.X, 0, Speed);
                velocityTemp.Y = Mathf.MoveToward(Body.Velocity.Y, 0, Speed);
            }

            Body.Velocity = velocityTemp;
            Body.MoveAndSlide();
        }

    }
}
