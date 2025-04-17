using Godot;
using LucidSpiral.Globals;
using System;

public partial class StartButton : Button
{
    [Export] public string ScenePath = "";
    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile(ScenePath);
        Utils.UnPauseGame();
    }
}
