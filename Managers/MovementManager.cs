using Godot;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LucidSpiral.MovementPatterns;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using System.Diagnostics;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class MovementManager : Node, IManager
    {
        public List<MovementPattern> MovementPatterns { get; private set; } = new();
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
            Debug.Assert(MovementPatterns.Count > 0);
            ActiveMovementPattern = MovementPatterns[0];
        }
        public override void _Process(double delta)
        {
            ActiveMovementPattern.Move();
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
        public void SetMovementPattern<T>()
        {
            foreach(MovementPattern pattern in MovementPatterns)
            {
                if (pattern is T)
                {
                    ActiveIndex = MovementPatterns.IndexOf(pattern);
                    ActiveMovementPattern = MovementPatterns[ActiveIndex];
                }
            }
        }
        public void ResetMovementCycle()
        {
            ActiveIndex = 0;
            ActiveMovementPattern = MovementPatterns[ActiveIndex];
        }
    }
}
