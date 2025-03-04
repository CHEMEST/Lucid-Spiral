using System;
using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerUtils;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using LucidSpiral.Statuses;

namespace LucidSpiral.Behaviors.Actions
{
    [GlobalClass]
    internal partial class PlayerDash : ActionPattern
    {
        [Export] private int dashesForHyper = 3;
        [Export] private float dashSpeed = 1200f;
        [Export] private float hyperDashMult = 2f;
        [Export] private float dashTime = 0.4f;
        [Export] private float dashCooldown = 2f;

        private int dashesUntilHyper;
        private float dashTimer = 0f;
        private float cooldownTimer = 0f;
        private bool isDashing = false;
        private bool isHyper = false;

        public override void _Ready()
        {
            base._Ready();
            dashesUntilHyper = dashesForHyper;
        }

        public override void Action(double delta)
        {
            GD.Print(Utils.FindManager<StateManager>(Source).ActiveState);
            if (cooldownTimer > 0)
            {
                cooldownTimer -= (float)delta;
            }

            if (isDashing)
            {
                dashTimer -= (float)delta;
                if (dashTimer <= 0)
                {
                    EndDash();
                }
            }
            else if (Input.IsActionJustPressed("Dash") && cooldownTimer <= 0)
            {
                if (isHyper) StartHyperDash();
                else StartDash();
            }
        }

        private void StartHyperDash()
        {
            dashSpeed *= hyperDashMult;
            StartDash();
            dashSpeed /= hyperDashMult;
            isHyper = false;
            dashesUntilHyper = dashesForHyper;
        }

        private void StartDash()
        {
            //setting state
            Utils.FindManager<StateManager>(Source).ActiveState = State.Dashing;

            isDashing = true;
            dashTimer = dashTime;
            cooldownTimer = dashCooldown;
            // Decrement untill hyper
            dashesUntilHyper--;
            if (dashesUntilHyper <= 0) isHyper = true;

            // Cutting active movement during dash
            MovementPattern movement = (MovementPattern) Utils.FindManager<MovementManager>(Source).GetActiveBehavior();
            movement.CanMove = false;

            Player player = Global.Player;

            Vector2 mousePosition = player.GetGlobalMousePosition();
            Vector2 playerPosition = player.GlobalPosition;
            Vector2 dashDirection = playerPosition.DirectionTo(mousePosition);

            player.Velocity = dashDirection * dashSpeed;

            player.MoveAndSlide();
        }

        private void EndDash()
        {
            //resetting state
            Utils.FindManager<StateManager>(Source).ActiveState = State.Idle;

            isDashing = false;
            // Re-enable movement
            MovementPattern movement = (MovementPattern)Utils.FindManager<MovementManager>(Source).GetActiveBehavior();
            movement.CanMove = true;
        }
    }
}
