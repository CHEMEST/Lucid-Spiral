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
    partial class EntryPoint : Node2D
    {
        [Export] public Direction Direction { get; private set; }
        public Room Room { get; private set; }
        public Vector2I Dir
        {
            get { return directionVectors[Direction]; }

        }
        public Direction GetOppositeDirection()
        {
            return Direction switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                _ => Direction.Null, // default
            };
        }
        public override void _Ready()
        {
            base._Ready();
            Debug.Assert(Direction != Direction.Null, Name + " has unset direction");
            Debug.Assert(GetParent() is Room, Name + "'s parent is not a " + nameof(Room));
            Room = GetParent() as Room;
        }


        private static readonly Dictionary<Direction, Vector2I> directionVectors = new Dictionary<Direction, Vector2I>
    {
        { Direction.North, new Vector2I(0, -1) },
        { Direction.East, new Vector2I(1, 0) },
        { Direction.South, new Vector2I(0, 1) },
        { Direction.West, new Vector2I(-1, 0) },
        { Direction.Null, new Vector2I(0, 0) }
    };

        public override string ToString()
        {
            return "{(" + GlobalPosition.X + ", " + GlobalPosition.Y + "), " + Direction + "}";
        }

    }
}
