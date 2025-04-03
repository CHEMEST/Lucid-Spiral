using Godot;
using System;
[GlobalClass]
public partial class NavCam : Camera2D
{
    [Export] public float ZoomSpeed = 0.01f;
    [Export] public float MinZoom = 0.005f;
    [Export] public float MaxZoom = 5.0f;
    [Export] public float PanSpeed = 1.0f;

    private Vector2 dragStart;
    private bool dragging = false;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {
                Zoom = (Zoom + new Vector2(ZoomSpeed, ZoomSpeed)).Clamp(new Vector2(MinZoom, MinZoom), new Vector2(MaxZoom, MaxZoom));
            }
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
                Zoom = (Zoom - new Vector2(ZoomSpeed, ZoomSpeed)).Clamp(new Vector2(MinZoom, MinZoom), new Vector2(MaxZoom, MaxZoom));
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                dragging = mouseEvent.Pressed;
                if (dragging)
                {
                    dragStart = GetGlobalMousePosition();
                }
            }
        }
        else if (@event is InputEventMouseMotion motionEvent && dragging)
        {
            Vector2 dragEnd = GetGlobalMousePosition();
            Position -= (dragEnd - dragStart) * PanSpeed;
            dragStart = dragEnd;
        }
    }
}