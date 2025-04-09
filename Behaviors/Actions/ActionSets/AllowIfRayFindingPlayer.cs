using Godot;
using LucidSpiral.Behaviors.Actions.ActionUtils;
using LucidSpiral.Behaviors.MovementPatterns.MovementUtils;
using LucidSpiral.Entities.Creatures;
using LucidSpiral.Globals;
using LucidSpiral.Managers;
using LucidSpiral.MovementPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Behaviors.Actions.ActionSets
{
    [GlobalClass]
    internal partial class AllowIfRayFindingPlayer : ActionSet
    {
        public AllowIfRayFindingPlayer() { }
        public override void _Ready()
        {
            base._Ready();
            // Connect to the RayfindPlayer's VisionChanged signal
            CallDeferred("Init");
        }

        private void Init()
        {
            Entity entity = Utils.FindEntityCarrying(this);
            MovementManager movementManager = Utils.FindManager<MovementManager>(entity);
            IMovement movement = movementManager.GetActiveBehavior();
            if (movement is RayfindPlayer rayfindPlayer)
            {
                rayfindPlayer.VisionChanged += OnVisionChanged;
            }
            else
            {
                GD.PrintErr("AllowIfRayFindingPlayer: No RayfindPlayer found in the active movement pattern.");
            }
        }

        private void OnVisionChanged(bool isPlayerVisible)
        {
            this.Enabled = isPlayerVisible;
            GD.Print("Recieved: " +  isPlayerVisible);
        }
    }
}
