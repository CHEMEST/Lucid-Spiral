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
using LucidSpiral.Globals;

namespace LucidSpiral.MovementPatterns
{
    [GlobalClass]
    internal partial class ChasePlayer : MovementPattern
    {
        [Export] private float stopDistance = 150.0f;
        [Export] private float slowingSpeed = 5f;

        public ChasePlayer() { }
        public override void Move(double delta)
        {
            float speed = (float)Utils.FindStatus<Speed>(Body).Value;
            speed = (float)(speed * Global.Random.NextDouble()) + (speed * 0.75f);
            CharacterBody2D Player = Global.Player;

            Vector2 velocityTemp = Body.Velocity;

            Vector2 direction = (Player.Position - Body.Position).Normalized();
            
            if (Body.Position.DistanceTo(Player.Position) > stopDistance)
            {
                velocityTemp = direction * speed * (float)delta;
            }
            else
            {
                velocityTemp.X = Mathf.MoveToward(Body.Velocity.X, 0, speed / slowingSpeed * (float)delta);
                velocityTemp.Y = Mathf.MoveToward(Body.Velocity.Y, 0, speed / slowingSpeed * (float)delta);
            }

            Body.Velocity = velocityTemp;
            Body.MoveAndSlide();
        }

    }
}
