using Godot;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class StatusManager : Node, IManager
    {
        public Dictionary<Type, Status<object>> Statuses { get; set; } = new();

        public override void _Ready()
        {

        }

        public void AddStatus(Status<object> status, bool overrideExisting = false)
        {
            // if override or (not exists)
            if (overrideExisting || !Statuses.ContainsKey(status.GetType()))
            {
                Statuses[status.GetType()] = status;
            }
        }

        public T GetStatus<T>() where T : Status<object>
        {
            if (Statuses.TryGetValue(typeof(T), out Status<object> status))
            {
                return status as T;
            }
            return EmptyStatus.Instance as T;
        }
    }
}
