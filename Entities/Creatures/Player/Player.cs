using Godot;
using LucidSpiral.Behaviors.Actions;
using LucidSpiral.Behaviors.Actions.ActionUtils;
using LucidSpiral.Entities.Creatures;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;

public partial class Player : Entity
{
    [Export] private PackedScene deathScreen;
    [Export] private Camera2D camera;
    public override void _Ready()
    {
        Global.Player = this;
        Global.Camera = camera;
        Utils.FindManager<StatusManager>(this).GetStatus<Health>().HealthDepleted += PlayerDeath;
    }

    private void PlayerDeath()
    {
        Utils.PauseGame();
        Global.Main.Menu.AddChild(deathScreen.Instantiate());
        // load death screen and 
    }

    public override void Kill()
    {
        PlayerDeath();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        //GD.Print(GlobalPosition);
        //GD.Print(this);
        //GD.Print(Utils.FindStatus<Health>(this));
        //GD.Print(Utils.FindManager<EffectManager>(this));
    }

    public override string ToString()
    {
        string log = "Player {";
        foreach (Node node in GetNodeOrNull("ManagerHub").GetChildren())
        {
            if (node is IManager manager)
            {
                log += " | " + manager.GetManagerName();

            }
        }
        log += "}";
        return log;
    }

    internal void StartTrickle()
    {
        Utils.FindManager<ActionManager>(this).GetNode<ActionSet>("LuciditySet").GetNode<LucidityTrickle>("LucidityTrickle").Started = true;
    }
}
