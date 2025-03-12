using Godot;
using LucidSpiral.Globals;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using LucidSpiral.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.MovementPatterns
{
    [GlobalClass]
    internal partial class StraightLineMotionToPlayer : MovementPattern
    {
        private Vector2 direction;
        public StraightLineMotionToPlayer() { }
        public override void _Ready()
        {
            base._Ready();
            CharacterBody2D Player = Global.Player;
            direction = (Player.Position - Body.Position).Normalized();
        }
        public override void Move(double delta)
        {
            float speed = (float)Utils.FindStatus<Speed>(Body).Value;

            Body.Velocity = direction * speed;
            Body.MoveAndSlide();
        }
    }
}
