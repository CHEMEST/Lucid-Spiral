﻿using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Entities.Creatures;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Weapons.WeaponUtils
{
    internal abstract partial class Weapon : Entity, IWeapon
    {
        [Export] public CharacterBody2D WeaponSource { get; private set; }
        [Export] public AnimationPlayer AnimationPlayer { get; private set; }
        [Export] public bool IsActive { get; set; } = true;
        private WeaponActionManager ActionManager;
        public override void _Ready()
        {
            base._Ready();
            if (WeaponSource == null)
            {
                Node owner = GetOwner();
                if (owner is CharacterBody2D)
                {
                    WeaponSource = owner as CharacterBody2D;
                }
            }
            Debug.Assert(WeaponSource != null, "Weapon missing a CharacterBody2D Body to Act upon");
            if (AnimationPlayer == null)
            {
                AnimationPlayer = WeaponSource.GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
            }
            if (AnimationPlayer == null)
            {
                GD.PrintErr("Weapon missing AnimationPlayer");
            }
            ActionManager = Utils.FindManager<WeaponActionManager>(this);

            CollisionSet collisionSet = Utils.FindCollisionSet(this, CollisionType.Hitbox);
            CollisionSet collisionSet2 = Utils.FindCollisionSet(this, CollisionType.SecondaryHitbox);
            if (collisionSet2 != null)
            {
                collisionSet2.Ignoring.Add(WeaponSource);
                GD.Print("Adding ignore to secondary hitbox");
            }
            collisionSet.Ignoring.Add(WeaponSource);
        }
        public void Act(double delta)
        {
            if (CanBasicAttack())
            {
                Utils.SetState(WeaponSource, Managers.ManagerUtils.State.Attacking);
                AnimationPlayer.Play("BasicAttack");
                ActionManager.BasicAttack.Action(delta);
            }
            if (CanSpecialAttack())
            {
                Utils.SetState(WeaponSource, Managers.ManagerUtils.State.Attacking);
                AnimationPlayer.Play("SpecialAttack");
                ActionManager.SpecialAttack.Action(delta);
            }
        }
        // can be overridden if a specific weapon needs different reqs
        private bool CanBasicAttack()
        {
            return Input.IsActionJustPressed("Basic Attack");
        }
        private bool CanSpecialAttack()
        {
            return Input.IsActionJustPressed("Special Attack");
        }
    }
}
