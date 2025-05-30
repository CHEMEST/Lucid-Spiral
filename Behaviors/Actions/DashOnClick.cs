﻿using System;
using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerUtils;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using LucidSpiral.Statuses;

namespace LucidSpiral.Behaviors.Actions
{
    [GlobalClass]
    /// <summary>
    /// Makes player invincible during dash, every some number of dashes triggers a hyper dash (faster)
    /// </summary>
    internal partial class DashOnClick : ActionPattern
    {
        [Export] private int dashesForHyper = 3;
        [Export] private float dashSpeedMult = 4;
        [Export] private float hyperDashMult = 2f;
        [Export] private float dashTime = 0.2f;
        [Export] private float dashCooldown = 2f;

        private int dashesUntilHyper;
        private float dashTimer = 0f;
        private float cooldownTimer = 0f;
        private bool isDashing = false;
        private bool isHyper = false;
        private CollisionShape2D sourcePhysicsCollider;
        public override void _Ready()
        {
            base._Ready();
            sourcePhysicsCollider = Source.GetNode<CollisionShape2D>("PhysicsShape");
            dashesUntilHyper = dashesForHyper;
        }

        public override void Action(double delta)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= (float)delta;
            }

            if (isDashing)
            {
                DuringDash(delta);
            }
            else if (Input.IsActionJustPressed("Dash") && cooldownTimer <= 0)
            {
                StartDash();
            }
        }

        private void DuringDash(double delta)
        {
            dashTimer -= (float)delta;
            if (dashTimer <= 0)
            {
                EndDash();
            }

            Source.Velocity = Source.Velocity.MoveToward(Vector2.Zero, (float)delta * Global.dtk);
            Source.MoveAndSlide();
        }

        private void StartDash()
        {
            // setting state
            Utils.SetState(Source, State.Dashing);
            // invincibility & passthrough by turning off hitbox
            Utils.FindCollisionSet(Source, CollisionType.Hitbox).IsDetectable = false;
            Source.CollisionLayer = 2;
            // Cutting active movement during dash
            Utils.GetActiveMovementPattern(Source).CanMove = false;

            // Decrement untill hyper
            dashesUntilHyper--;
            if (dashesUntilHyper <= 0) isHyper = true;

            // Dash
            isDashing = true;
            dashTimer = dashTime;
            cooldownTimer = dashCooldown;

            // Calculating direction
            Vector2 mousePosition = Source.GetGlobalMousePosition();
            Vector2 playerPosition = Source.Position;
            Vector2 dashDirection = playerPosition.DirectionTo(mousePosition);


            // hyper
            float speedMult = (isHyper) ? dashSpeedMult * hyperDashMult : dashSpeedMult;
            if (isHyper) { 
                isHyper = false;
                dashesUntilHyper = dashesForHyper;
            }

            double sourceSpeed = Utils.FindStatus<Speed>(Source).Value;
            float speed = (float)sourceSpeed * speedMult;

            Source.Velocity = dashDirection * speed;
            Source.MoveAndSlide();
        }

        private void EndDash()
        {
            isDashing = false;
            //resetting state
            Utils.SetState(Source, State.Idle);
            // Re-enable hitbox
            Utils.FindCollisionSet(Source, CollisionType.Hitbox).IsDetectable = true;
            Source.CollisionLayer = 1;
            // Re-enable movement
            Utils.GetActiveMovementPattern(Source).CanMove = true;
        }
    }
}
