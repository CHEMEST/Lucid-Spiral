using Godot;
using LucidSpiral.Entities.Creatures;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;

public partial class Player : Entity
{
    [Export] private PackedScene deathScreen;
    public override void _Ready()
    {
        Global.Player = this;
        Utils.FindManager<StatusManager>(this).GetStatus<Health>().HealthDepleted += PlayerDeath;
    }

    private void PlayerDeath()
    {
        Engine.Singleton.TimeScale = 0;
        Utils.DisableSubtree(Global.Main.World);
        Utils.DisableSubtree(Global.Main.GUI);
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

}
