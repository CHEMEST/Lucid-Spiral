using Godot;
using LucidSpiral.MapTools.MapUtils;
using System;

public partial class MapManager : Node2D
{
    public override void _Ready()
    {
        base._Ready();
    }
    public void SwitchRoom(EntryPoint entryPoint)
    {
        GD.Print(entryPoint.Dir);
    }
}
