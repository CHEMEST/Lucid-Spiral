using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.MapTools.MapUtils
{
    [GlobalClass]
    partial class Room : Node2D
    {
        private List<EntryPoint> entries { get; } = new();
        public override void _Ready()
        {
            base._Ready();

            foreach (Node child in GetChildren())
            {
                if (child is EntryPoint entry)
                {
                    entries.Add(entry);
                }
            }
            Debug.Assert(entries.Count > 0, Name + " is missing an " + nameof(EntryPoint));
        }
    }
}
