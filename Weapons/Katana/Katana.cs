using Godot;
using LucidSpiral.Behaviors.Weapons.WeaponUtils;
using LucidSpiral.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Weapons
{
    [GlobalClass]
    internal partial class Katana : Weapon
    {
        public override void BasicAttack(double delta)
        {
            Utils.ProcessCollisions(this, Collisions.CollisionUtils.CollisionType.Hitbox, Collisions.CollisionUtils.CollisionType.Hitbox, on =>
            {
                GD.Print("Making an attack with Katana on " + on.GetOwner().Name);
            });            
        }

        public override void SpecialAttack(double delta)
        {
            throw new NotImplementedException();
        }
    }
}
