using Godot;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using System.Diagnostics;
using LucidSpiral.Managers.ManagerUtils;
using LucidSpiral.Behaviors.MovementPatterns.MovementUtils;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class MovementManager : FocusedManager<IMovement>
    {
    }
}
