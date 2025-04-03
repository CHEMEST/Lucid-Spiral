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
    internal partial class PathfindPlayer : MovementPattern
    {
        private RayCast2D raycast;

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
            Body.AddChild(raycast); // error here, call defer maybe?
        }
        // worked with CGPT to make this
        // PS. make this a cone later for better vision and make the body move to the last location of the player if not found
        public override void Move(double delta) // Called externally every physics process
        {
            float speed = (float)Utils.FindStatus<Speed>(Body).Value;
            float dt = (float)delta * Global.dtk;
            CharacterBody2D Player = Global.Player;

            // Update the RayCast2D to point toward the player
            Vector2 directionToPlayer = (Player.GlobalPosition - Body.GlobalPosition).Normalized();
            raycast.TargetPosition = directionToPlayer * 20f; // Extend ray to a reasonable distance
            raycast.ForceRaycastUpdate();

            // Check if the enemy has a clear line of sight to the player
            if (!raycast.IsColliding() || raycast.GetCollider() == Player)
            {
                // Move towards the player if visible
                Body.Velocity = directionToPlayer * speed;
            }
            else
            {
                // Implement pathfinding or alternative behavior when the player isn't visible
                Body.Velocity = Vector2.Zero; // Stop movement or use pathfinding instead
            }

            Body.MoveAndSlide();
        }
    }
}
