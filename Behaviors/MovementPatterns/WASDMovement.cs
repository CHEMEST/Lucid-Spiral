using Godot;
using LucidSpiral.Globals;
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
    internal partial class WASDMovement : MovementPattern
    {
        public WASDMovement() { }
        public override void Move(double delta)
        {
            float speed = (float)Utils.FindStatus<Speed>(Body).Value;
            
            Vector2 velocityTemp = Body.Velocity;

            Vector2 direction = Input.GetVector("Move Left", "Move Right", "Move Up", "Move Down");
            if (direction != Vector2.Zero)
            {
                velocityTemp = direction * speed;
            }
            else
            {
                velocityTemp = velocityTemp.MoveToward(Vector2.Zero, speed*(float)delta);
            }

            Body.Velocity = velocityTemp;
            Body.MoveAndSlide();
        }

    }
}
