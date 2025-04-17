using Godot;
using System;

public partial class SwitchSceneOnPress : Button
{
    [Export] public string ScenePath = "";
    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile(ScenePath);
    }
}
