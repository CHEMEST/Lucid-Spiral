using Godot;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using LucidSpiral.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.MovementPatterns
{
    [GlobalClass]
    internal partial class DefaultPlayerWASD : MovementPattern
    {
        public DefaultPlayerWASD() { }
        public override void Move()
        {
            float Speed = GetNode<ManagerHub>("../..").GetManager<StatusManager>().GetStatus<Speed>().Value;

            
            Vector2 velocityTemp = Body.Velocity;

            Vector2 direction = Input.GetVector("Move Left", "Move Right", "Move Up", "Move Down");
            if (direction != Vector2.Zero)
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
