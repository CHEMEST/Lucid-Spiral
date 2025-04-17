using Godot;
using System;

public partial class Bed : Node2D
{
    [Export] private PackedScene map;
    private Node2D _mapInstance;
    [Export] private Area2D area;

    public override void _Ready()
    {
        area.AreaEntered += OnAreaEntered;
    }

    private async void OnAreaEntered(Area2D area)
    {
        if (area.GetOwner() is Player player)
        {
            await ToSignal(GetTree(), "process_frame");
            _mapInstance = map.Instantiate<Node2D>();
            Global.Main.World.AddChild(_mapInstance);
            Global.Camera.Zoom = new Vector2(1, 1);
            CallDeferred(nameof(CleanUp));
        }
    }
    private void CleanUp()
    {
        foreach (Node child in Global.Main.World.GetChildren())
        {
            if (child != _mapInstance)
            {
                child.QueueFree();
            }
        }
    }
}
