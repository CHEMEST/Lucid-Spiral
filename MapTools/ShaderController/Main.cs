using Godot;
using System;
using LucidSpiral.Globals;
using LucidSpiral.MapTools.MapUtils;


public partial class Main : Node2D
{
    [Export] public CanvasLayer Menu { get; private set; }
    [Export] public CanvasLayer Shader { get; private set; }
    [Export] public CanvasLayer GUI { get; private set; }
    [Export] public Node2D World { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        Global.Main = this;
    }

}
