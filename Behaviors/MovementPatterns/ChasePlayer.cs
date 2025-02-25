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
        [Export] private float stopDistance = 150.0f;
        [Export] private float slowingSpeed = 20f;

        public ChasePlayer() { }
        public override void Move(double delta)
        {
            float speed = (float)GetNode<ManagerHub>("../..").GetManager<StatusManager>().GetStatus<Speed>().Value;
            CharacterBody2D Player = Global.Player;

            Vector2 velocityTemp = Body.Velocity;

            Vector2 direction = (Player.Position - Body.Position).Normalized();
            
            if (Body.Position.DistanceTo(Player.Position) > stopDistance)
            {
                velocityTemp = direction * speed;
            }
            else
            {
                velocityTemp.X = Mathf.MoveToward(Body.Velocity.X, 0, speed / slowingSpeed);
                velocityTemp.Y = Mathf.MoveToward(Body.Velocity.Y, 0, speed / slowingSpeed);
            }

            Body.Velocity = velocityTemp;
            Body.MoveAndSlide();
        }

    }
}
