using Godot;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;

public partial class LucidityBar : ProgressBar
{
    private bool initialized = false;
    private void init()
    {
        
        Health lucidity = Utils.FindManager<StatusManager>(Global.Player).GetStatus<Health>();
        lucidity.StatusChanged += LucidityChanged;
        this.MaxValue = lucidity.Max;
        LucidityChanged(lucidity.Value);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (initialized == true) return;
        if (Global.Player == null) return;
        else
        {
            init();
        }
    }

    private void LucidityChanged(double value)
    {
         this.Value = value;
    }
}
