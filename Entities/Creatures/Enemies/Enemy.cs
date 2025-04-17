using Godot;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Entities.Creatures.Enemies
{
    [GlobalClass]
    internal partial class Enemy : Entity
    {
        [Export] public EnemyValue Value = EnemyValue.None; 
        public override void Kill()
        {
            Global.Score += (int)Value;
            Utils.FindStatus<Health>(Global.Player).Modify((v) => v + (int)Value);
            QueueFree();
        }
        public override void _Ready()
        {
            base._Ready();
            CallDeferred(nameof(init));
        }
        private void init()
        {
            Health health = Utils.FindManager<StatusManager>(this).GetStatus<Health>();
            health.HealthDepleted += OnHealthDepleted;
        }
        public void OnHealthDepleted()
        {
            Kill();
        }
    }
}
