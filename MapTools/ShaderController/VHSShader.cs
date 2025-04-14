using Godot;
using System;

public partial class VHSShader : TextureRect
{
    private float _time = 0f;
    public override void _Process(double delta)
    {
        _time += (float)delta;
        Material.Set("time", _time);
    }
}
