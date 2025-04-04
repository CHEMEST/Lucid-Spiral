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
    internal partial class RayfindPlayer : MovementPattern
    {
        private RayCast2D raycast;
        [Export] private float VisionRange = 500f;

        public override void _Ready()
        {
            base._Ready();

            raycast = new RayCast2D
            {
                TargetPosition = Vector2.Zero,
                Enabled = true,
                CollideWithAreas = false,
                CollideWithBodies = true
            };

            AddChild(raycast);
        }
        public override void _Process(double delta)
        {
            QueueRedraw();
        }

        public override void _Draw()
        {

            Vector2 endPoint = Body.ToLocal(raycast.GlobalPosition + raycast.TargetPosition);
            DrawLine(Vector2.Zero, endPoint, Colors.Red, 2);
        }
        // worked with CGPT to make this
        // PS. make this a cone later for better vision and make the body move to the last location of the player if not found
        public override void Move(double delta) // Called externally every physics process
        {
            float maxSpeed = (float)Utils.FindStatus<Speed>(Body).Value;
            float dt = (float)delta * Global.dtk;
            CharacterBody2D Player = Global.Player;
            Vector2 toPlayer = Player.GlobalPosition - Body.GlobalPosition;
            float distance = toPlayer.Length();

            if (distance > VisionRange)
            {
                // Player is out of vision range
                Body.Velocity = Body.Velocity.MoveToward(Vector2.Zero, dt);
                Body.MoveAndSlide();
                return;
            }

            // Point raycast directly to the player (within range)
            Vector2 direction = toPlayer.Normalized();
            raycast.TargetPosition = direction * distance;
            raycast.ForceRaycastUpdate();


            // Check if the enemy has a clear line of sight to the player
            if (!raycast.IsColliding() || raycast.GetCollider() == Player)
            {
                // Move towards the player if visible
                Body.Velocity = Body.Velocity.MoveToward(direction * maxSpeed, dt);
            }
            else
            {
                Body.Velocity = Body.Velocity.MoveToward(Vector2.Zero, dt);
            }

            Body.MoveAndSlide();
        }
    }
}
