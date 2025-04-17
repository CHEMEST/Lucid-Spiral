using Godot;
using System;

public partial class AutoPlayAnim : AnimatedSprite2D
{
    public override void _Ready()
    {
        base._Ready();
        Play();
        
    }
}
