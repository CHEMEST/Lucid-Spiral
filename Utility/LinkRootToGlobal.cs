using Godot;
using System;
using LucidSpiral.Globals;
using LucidSpiral.MapTools.MapUtils;


public partial class LinkRootToGlobal : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        Global.Root = this;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouse mouseEvent)
        {
            Global.Instance.HandleMouseInput(mouseEvent);
        }
    }

}
