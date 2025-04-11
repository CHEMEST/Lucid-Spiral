using Godot;
using System;

public partial class Global : Node
{
    [Signal] public delegate void PlayerLoadedEventHandler();
    public static Global instance;
    public static Player Player;
    public static float dtk = 1000f; // delta time const
    public static Random Random = new Random();

    private bool playerInitialized = false;
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
