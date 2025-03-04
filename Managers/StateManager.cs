using Godot;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Managers.ManagerUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class StateManager : Node, IManager
    {
        [Signal]
        public delegate void StateChangedEventHandler(int newState);

        private State _activeState = State.Idle;

        public State ActiveState
        {
            get => _activeState;
            set
            {
                if (_activeState != value) // Only update if the state is actually changing
                {
                    _activeState = value;
                    EmitSignal(SignalName.StateChanged, (int)_activeState);
                }
            }
        }
    }
}
