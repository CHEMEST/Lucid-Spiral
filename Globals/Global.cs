using Godot;
using System;

public partial class Global : Node
{
    [Signal] public delegate void PlayerLoadedEventHandler();
    public static Global Instance { get; private set; }

    public static Player Player;
    public static int Score = 0;

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
    public void HandleMouseInput(InputEventMouseButton mouseEvent)
    {
        if (Root == null) return;

        Vector2 globalMouse = mouseEvent.GlobalPosition;
        MousePos = globalMouse;
        var screenRect = Root.GetChild<TextureRect>(0).GetGlobalRect();

        Vector2 localInViewport = (globalMouse - screenRect.Position) / screenRect.Size * (Root.FindChild("SubViewport") as SubViewport).Size;

        MousePos = localInViewport;  

        //GD.Print($"Mouse {mouseEvent.ButtonIndex} clicked at: {MousePos}");
    }


}
