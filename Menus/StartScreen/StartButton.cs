using Godot;
using System;

public partial class StartButton : Button
{
    [Export] public string LobbyPath = "";
    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile(LobbyPath);
    }
}
