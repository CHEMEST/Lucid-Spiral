using Godot;
using System.Collections.Generic;
[GlobalClass]
public partial class RoomInstance : Node2D
{
    public List<Node2D> Exits = new List<Node2D>();

    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Node2D exit)
                Exits.Add(exit);
        }
    }

    public bool Overlaps(RoomInstance other)
    {
        Area2D myArea = GetNodeOrNull<Area2D>("RoomCollider");
        Area2D otherArea = other.GetNodeOrNull<Area2D>("RoomCollider");

        if (myArea == null || otherArea == null)
            return false;

        // Check if the two Area2D nodes are overlapping
        return myArea.OverlapsArea(otherArea);
    }

    public Node2D GetOppositeExit(Node2D exit)
    {
        return Exits.Find(e => e.GlobalPosition.DistanceTo(exit.GlobalPosition) < 5);
    }
}
