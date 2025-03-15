using Godot;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Managers.ManagerUtils;
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
    internal partial class PlayerMovement : MovementPattern
    {
        [Export] private float frictionConst = 3;
        public PlayerMovement() { }
        public override void Move(double delta)
        {
            float speed = (float)Utils.FindStatus<Speed>(Body).Value;
            float friction = speed * frictionConst;
            float dt = Global.dtk * (float)delta;
            
            Vector2 velocityTemp = Body.Velocity;

            Vector2 direction = Input.GetVector("Move Left", "Move Right", "Move Up", "Move Down");

            if (direction != Vector2.Zero)
            {
                velocityTemp = direction * speed;
                Utils.SetState(Body, State.Moving);
            }
            else
            {
                velocityTemp = velocityTemp.MoveToward(Vector2.Zero, friction * dt);
            }
            
            if (velocityTemp == Vector2.Zero)
            {
                Utils.SetState(Body, State.Idle);
            } 
            Body.Velocity = velocityTemp;
            Body.MoveAndSlide();
        }

    }
}
