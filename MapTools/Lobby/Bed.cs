using Godot;
using System;

public partial class Bed : Node2D
{
    [Export] private string MapPath = "";
    [Export] private Area2D Area;

    public override void _Ready()
    {
        Area.AreaEntered += OnAreaEntered;
    }

    private async void OnAreaEntered(Area2D area)
    {
        if (area.GetOwner() is Player player)
        {
            await ToSignal(GetTree(), "process_frame");
            GetTree().ChangeSceneToFile(MapPath);
        }
    }
}
