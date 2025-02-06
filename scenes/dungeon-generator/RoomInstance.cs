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

    public bool Overlaps(List<Area2D> others)
    {
        Area2D myArea = GetNodeOrNull<Area2D>("RoomCollider");

        foreach (Area2D other in others)
        {
            if (myArea.OverlapsArea(other))
            {
                return true;
            }
        }
        return false;
    }

    public Area2D getRoomCollider()
    {
        return GetNodeOrNull<Area2D>("RoomCollider");
    }

    public Node2D GetOppositeExit(Node2D exit)
    {
        return Exits.Find(e => e.GlobalPosition.DistanceTo(exit.GlobalPosition) < 5);
    }
}
