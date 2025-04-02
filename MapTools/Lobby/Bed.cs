using Godot;
using System;

public partial class Bed : Area2D
{
    [Export] public string MapPath = "";

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
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
