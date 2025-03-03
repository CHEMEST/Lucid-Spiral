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
    internal partial class PlayerMovement : MovementPattern
    {
        [Export] public float DashSpeed = 1800f;
        [Export] public float DashCooldown = 3.0f;

        private float _cooldownTimer = 0f;

        private Vector2 velocityTemp = Vector2.Zero;

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (_cooldownTimer >= 0)
                _cooldownTimer -= (float)delta;
        }
        private void DashTowardsMouse()
        {
            Vector2 mousePosition = GetViewport().GetMousePosition();
            Vector2 _dashDirection = (mousePosition - Body.GlobalPosition).Normalized();
            GD.Print(mousePosition, Body.Position);
            velocityTemp = _dashDirection * DashSpeed;
            _cooldownTimer = DashCooldown;
        }
        public PlayerMovement() { }
        public override void Move(double delta)
        {
            float speed = (float)Utils.FindStatus<Speed>(Body).Value;
            float friction = speed * 2;
            
            velocityTemp = Body.Velocity;

            Vector2 direction = Input.GetVector("Move Left", "Move Right", "Move Up", "Move Down");

            if (direction != Vector2.Zero)
            {
                velocityTemp = direction * speed;
            }
            else
            {
                velocityTemp = velocityTemp.MoveToward(Vector2.Zero, friction*(float)delta);
            }
            if (Input.IsActionJustPressed("Dash"))
            {
                if (_cooldownTimer < 0)
                {
                    DashTowardsMouse();
                }
            }
            Body.Velocity = velocityTemp;
            Body.MoveAndSlide();
        }

    }
}
