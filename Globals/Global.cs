using Godot;
using LucidSpiral.Globals;
using System;

public partial class Global : Node
{
    [Signal] public delegate void PlayerLoadedEventHandler();
    public static Global Instance { get; private set; }

    public static Player Player;
    public static int Score = 0;

    public static float dtk = 1000f; // delta time const
    public static Random Random = new Random();

    public static Main Main;
    public static Camera2D Camera;

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


}
