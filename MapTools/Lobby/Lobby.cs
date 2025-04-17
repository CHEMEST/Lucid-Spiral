using Godot;
using System;

public partial class Lobby : Node2D
{
    [Export] private Node2D playerSpawn;
    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(InitializePlayer));
    }
    public void InitializePlayer()
    {
        Global.Player.GlobalPosition = playerSpawn.GlobalPosition;
        Global.Camera.Zoom = new Vector2(0.65f, 0.65f);
    }
}
