using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Effects.EffectUtils
{
    internal abstract partial class Effect : Node2D, IEffect
    {
        public Effect()
        {

        }
        public void Act(double delta)
        {
            Affect(delta);
        }
        public abstract void Affect(double delta);
        public void EntryEffect() { }
        public void ExitEffect() { }

        public override void _Ready()
        {
            base._Ready();
            EntryEffect();
        }
        public void Remove()
        {
            ExitEffect();
            QueueFree();
        }


    }
}
