using Godot;
using System;

public partial class Bed : Area2D
{
    [Export] public string MapPath = "";

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.GetOwner() is Player player)
            GetTree().ChangeSceneToFile(MapPath);
    }
}
