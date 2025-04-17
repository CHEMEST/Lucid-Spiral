using Godot;
using System;

public partial class ScoreLabel : RichTextLabel
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        Text = "Score: " + Global.Score;
    }
}
