using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions
{
    [GlobalClass]
    internal partial class ProjectileSummon : ActionPattern
    {
        [Export] private PackedScene projectileScene = GD.Load<PackedScene>("res://Entities/Projectiles/BasicProjectile.tscn");
        public override void Action(double delta)
        {
            CharacterBody2D projectile = projectileScene.Instantiate() as CharacterBody2D;
            projectile.Position = Source.Position;
            projectile.Rotation = Source.Rotation;
            Source.GetParent().AddChild(projectile);
            // ignore source
            Utils.FindManager<CollisionManager>(projectile).GetCollisionSet(CollisionType.Hitbox).Ignoring.Add(Source);
        }
    }
}
