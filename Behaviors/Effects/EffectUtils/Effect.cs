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
        private Timer _lifeTimer;
        private Timer _delayTimer;

        /// <param name="lifespanS"> values less than or equal to 0 means indefinite </param>
        /// <param name="delayS"></param>
        /// <param name="auto"></param>
        public Effect(double lifespanS, double delayS, bool auto = true)
        {
            
            _delayTimer = new Timer
            {
                WaitTime = delayS,
                Autostart = auto,
                OneShot = false
            };            
            _delayTimer.Timeout += Affect;
            if (lifespanS > 0)
            {
                _lifeTimer = new Timer
                {
                    WaitTime = lifespanS,
                    Autostart = true,
                };
                _lifeTimer.Timeout += End;
            }
        }

        /// <summary>
        /// Affect() should be able to assume that the Effect node is placed in an EffectManager of the thing it is affecting
        /// </summary>
        public abstract void Affect();
        public void EntryEffect() { }
        public void ExitEffect() { }

        public override void _Ready()
        {
            base._Ready();
            AddChild(_delayTimer);
            AddChild(_lifeTimer);
            EntryEffect();
        }
        public void End()
        {
            ExitEffect();
            QueueFree();
        }


    }
}
