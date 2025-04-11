using Godot;
using System;
using LucidSpiral.Globals;


public partial class LinkRootToGlobal : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        Global.Root = this;
    }
}
 