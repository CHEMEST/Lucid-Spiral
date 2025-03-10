using Godot;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class WeaponActionManager : Node2D, IManager
    {
        [Export] public ActionPattern BasicAttack { get; private set; }
        [Export] public ActionPattern SpecialAttack { get; private set; }

        public override void _Ready()
        {
            base._Ready();
            Debug.Assert((BasicAttack != null && SpecialAttack != null) || GetChildren().Count >= 2, "A weapon is missing an attack");

            if (BasicAttack == null)
            {
                Node kid = GetChildren()[0];
                if (kid is ActionPattern)
                {
                    BasicAttack = kid as ActionPattern;
                }
            }

            if (SpecialAttack == null)
            {
                Node kid = GetChildren()[1];
                if (kid is ActionPattern)
                {
                    SpecialAttack = kid as ActionPattern;
                }
            }
        }

    }
}
