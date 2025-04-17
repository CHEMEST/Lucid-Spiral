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
        Global.Instance.PlayerLoaded += Init;
    }
    private void Init()
    {
        Trickle trickle = Utils.FindManager<StatusManager>(Global.Player).GetStatus<Trickle>();
        trickle.StatusChanged += TrickleChanged;
        this.MaxValue = trickle.Max;
        TrickleChanged(trickle.Value);
    }

    private void TrickleChanged(double value)
    {
         this.Value = value;
    }
}
