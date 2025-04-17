using Godot;
using LucidSpiral.Behaviors.Weapons.WeaponUtils;
using LucidSpiral.Globals;
using LucidSpiral.Managers.ManagerUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class WeaponManager : FocusedManager<IWeapon>
    {
        [Export] private float maxSwordDistance = 200f;
        private Entities.Creatures.Entity source;
        public override void _Ready()
        {
            base._Ready();
            source = Utils.FindEntityCarrying(this);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            IWeapon iweapon = GetActiveBehavior();
            if (iweapon == null) return;
            if (iweapon is Node2D weapon)
            {
                Vector2 mousePos = GetGlobalMousePosition();
                if ( source.GlobalPosition.DistanceTo(mousePos) < maxSwordDistance )
                {
                    weapon.GlobalPosition = GetGlobalMousePosition();
                }
                else
                {
                    Vector2 toMouse = (GetGlobalMousePosition() - source.GlobalPosition).LimitLength(maxSwordDistance);
                    weapon.GlobalPosition = source.GlobalPosition + toMouse;
                }
            }
        }
    }
}
