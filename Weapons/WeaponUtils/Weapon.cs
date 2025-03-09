using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Globals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Weapons.WeaponUtils
{
    internal abstract partial class Weapon : Node2D, IWeapon
    {
        [Export] public CharacterBody2D Source { get; private set; }
        [Export] public bool IsActive { get; set; } = true;
        public override void _Ready()
        {
            base._Ready();
            if (Source == null)
            {
                Node owner = GetOwner();
                if (owner is CharacterBody2D)
                {
                    Source = owner as CharacterBody2D;
                }
            }
            Debug.Assert(Source != null, "Weapon missing a CharacterBody2D Body to Act upon");
        }
        public void Act(double delta)
        {
            if (CanBasicAttack())
            {
                Utils.SetState(Source, Managers.ManagerUtils.State.Attacking);
                BasicAttack(delta);
            }
            if (CanSpecialAttack())
            {
                Utils.SetState(Source, Managers.ManagerUtils.State.Attacking);
                SpecialAttack(delta);
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

        public abstract void BasicAttack(double delta);
        public abstract void SpecialAttack(double delta);
    }
}
