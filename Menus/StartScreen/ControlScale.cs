using Godot;
using System;

public partial class ControlScale : Control
{
    private Vector2 baseSize = new Vector2(960, 540);

    public override void _Ready()
    {
        AdjustScale();
    }

    public override void _Process(double delta)
    {
        AdjustScale();
    }

    private void AdjustScale()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        float scaleFactor = Mathf.Min(screenSize.X / baseSize.X, screenSize.Y / baseSize.Y);

        Scale = new Vector2(scaleFactor, scaleFactor);
        Position = (screenSize - (baseSize * scaleFactor)) / 2;
    }
}
