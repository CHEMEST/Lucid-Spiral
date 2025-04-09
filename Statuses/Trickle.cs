using Godot;
using LucidSpiral.Statuses.Enums;
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
        private int idx = 0;
        // make these actual values later (using an exp eq)
        private List<double> trickleValues = new()
        {
            1,
            2,
            3,
            4
        };
        public double Value
        {
            get
            {
                return trickleValues[idx];
            }
        }
        public Trickle() { }
        public void IncreaseTrickle()
        {
            if (idx >= trickleValues.Count)
            {
                idx = 0;
            }
            else
            {
                idx++;
            }
        }
    }
}
