using Godot;
using LucidSpiral.Behaviors.Weapons.WeaponUtils;
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
        public override void _Process(double delta)
        {
            base._Process(delta);
            IWeapon iweapon = GetActiveBehavior();
            if (iweapon == null) return;
            if (iweapon is Node2D weapon)
            {
                weapon.GlobalPosition = Global.MousePos;
            }
        }
    }
}
