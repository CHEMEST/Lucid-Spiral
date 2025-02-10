using Godot;
using System.Collections.Generic;
public partial class RoomInstance : Node2D
{
    public List<Node2D> Exits = new List<Node2D>();

    public override void _Ready()
    {
        FindExits();
    }
    // Find Node2D named "Exits" and add all its children (Node2D) to Exits
    private void FindExits()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Node2D && child.Name.Equals("Exits"))
            {
                foreach (Node2D exit in child.GetChildren())
                {
                    Exits.Add(exit);
                }
            }
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
}
