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
    public partial class Room : Node2D
    {
        [Export] private Control container;
        public List<EntryPoint> Entries { get; private set; } = new();

        public Vector2 Size {
            get
            {
                return new Vector2(container.Size.X, container.Size.Y);
            } 
        }

        public override void _Ready()
        {
            base._Ready();

            foreach (Node child in GetChildren())
            {
                if (child is EntryPoint entry)
                {
                    Entries.Add(entry);
                }
            }
            Debug.Assert(Entries.Count > 0, Name + " is missing an " + nameof(EntryPoint));
        }

    }
}
