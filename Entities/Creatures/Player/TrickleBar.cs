using Godot;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Statuses;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;

public partial class TrickleBar : ProgressBar
{
    public override void _Ready()
    {
        base._Ready();
        CallDeferred("init");
    }
    private void init()
    {
        Trickle trickle = Utils.FindManager<StatusManager>(Utils.FindEntityCarrying(this)).GetStatus<Trickle>();
        trickle.StatusChanged += TrickleChanged;
        this.MaxValue = trickle.Max;
        TrickleChanged(trickle.Value);
    }

    private void TrickleChanged(double value)
    {
         this.Value = value;
    }
}
