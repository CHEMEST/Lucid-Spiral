using Godot;
using System;

public partial class Global : Node
{
    public static Player Player;
    public static float dtk = 1000f; // delta time const
    public static Random Random = new Random();
}
