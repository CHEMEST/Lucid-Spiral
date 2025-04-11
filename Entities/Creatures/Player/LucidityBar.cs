using Godot;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Statuses;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;

public partial class LucidityBar : ProgressBar
{
    public override void _Ready()
    {
        base._Ready();
        Global.Instance.PlayerLoaded += Init;
    }
    private void Init()
    {
        Health lucidity = Utils.FindManager<StatusManager>(Global.Player).GetStatus<Health>();
        lucidity.StatusChanged += LucidityChanged;
        this.MaxValue = lucidity.Max;
        LucidityChanged(lucidity.Value);
    }

    private void LucidityChanged(double value)
    {
        this.Value = value;
    }
}
