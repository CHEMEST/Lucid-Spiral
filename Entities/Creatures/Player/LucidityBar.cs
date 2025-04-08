using Godot;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;

public partial class LucidityBar : ProgressBar
{
    public override void _Ready()
    {
        base._Ready();
        CallDeferred("init");
    }
    private void init()
    {
        Health lucidity = Utils.FindManager<StatusManager>(Utils.FindEntityCarrying(this)).GetStatus<Health>();
        lucidity.StatusChanged += LucidityChanged;
        this.MaxValue = lucidity.Max;
        LucidityChanged(lucidity.Value);
    }

    private void LucidityChanged(double value)
    {
         this.Value = value;
    }
}
