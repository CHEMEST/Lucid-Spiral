using Godot;
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
        public void Kill()
        {
            Global.Score += (int)Value;
        }
    }
}
