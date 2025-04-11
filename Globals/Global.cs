using Godot;
using System;

public partial class Global : Node
{
    [Signal] public delegate void PlayerLoadedEventHandler();
    public static Global Instance { get; private set; }

    public static Player Player;
    public static float dtk = 1000f; // delta time const
    public static Random Random = new Random();
    public static Node2D Root;
    public static Vector2 MousePos = Vector2.Zero;

    private bool playerInitialized = false;
    public override void _Ready()
    { 
        base._Ready();
        Instance = this;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (playerInitialized == false)
        {
            if (Player != null)
            {
                playerInitialized = true;
                EmitSignal(SignalName.PlayerLoaded);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouse mouseEvent)
        {
            if (Root == null) return;
            // Convert global mouse position to local SubViewport coordinates
            Vector2 globalMouse = mouseEvent.GlobalPosition;
            var screenRect = Root.GetChild<TextureRect>(0).GetGlobalRect();

            Vector2 localInViewport = (globalMouse - screenRect.Position) / screenRect.Size * Root.GetChild<SubViewport>(0).Size;
            MousePos = localInViewport;
            
        }
    }

}
