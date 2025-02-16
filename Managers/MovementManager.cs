using Godot;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LucidSpiral.MovementPatterns;
using LucidSpiral.MovementPatterns.MovementPatternThings;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class MovementManager : Node, IManager
    {
        public List<MovementPattern> MovementPatterns { get; private set; }
        public MovementPattern ActiveMovementPattern { get; set; }
        public int ActiveIndex { get; private set; } = 0;
        public MovementManager() { }

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is MovementPattern)
                {
                    MovementPatterns.Add(child as MovementPattern);
                }
            }
            ActiveMovementPattern = MovementPatterns[0];
        }
        public void NextMovementPattern()
        {
            if (ActiveIndex < MovementPatterns.Count)
            {
                ActiveIndex++;
            }
            else
            {
                ActiveIndex = 0;
            }
            ActiveMovementPattern = MovementPatterns[ActiveIndex];
        }
        public void ResetMovementCycle()
        {
            ActiveIndex = 0;
            ActiveMovementPattern = MovementPatterns[ActiveIndex];
        }
    }
}
