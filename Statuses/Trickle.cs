using Godot;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Statuses
{
    [GlobalClass]
    internal partial class Trickle : Node2D, IStatus
    {
        [Signal] public delegate void StatusChangedEventHandler(double value);
        private int idx = 0;
        // make these actual values later (using an exp eq)
        private List<double> trickleValues = new()
        {
            1,
            2,
            4,
            6
        };
        public double Max
        {
            get { return trickleValues.Last(); }
        }
        public double Value
        {
            get
            {
                return trickleValues[idx];
            }
        }
        public Trickle() { }
        public void IncreaseTrickle(int repititions = 1)
        {
            DeltaTrickle(repititions);
        }
        public void DecreaseTrickle (int repititions = 1)
        {
            DeltaTrickle(-repititions);
        }
        public void DeltaTrickle(int idxDelta)
        {
            idx += idxDelta;
            if (idx <= 0)
            {
                idx = 0;
            }
            if (idx > trickleValues.Count)
            {
                idx = trickleValues.Count;
            }
            EmitSignal(SignalName.StatusChanged, Value);
        }

    }
}
