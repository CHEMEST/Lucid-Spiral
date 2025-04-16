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
        public Dictionary<Type, IStatus> Statuses { get; private set; } = new();

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is IStatus status)
                {
                    AddStatus(status);
                }
            }
            //GD.Print(Statuses);
        }
        public override void _Process(double delta)
        {
            //foreach (var status in Statuses)
            //{
            //    GD.Print(status.Value.ToString());
            //}
        }

        public void AddStatus(IStatus status, bool overrideExisting = false)
        {
            // if override or (not exists)
            if (overrideExisting || !Statuses.ContainsKey(status.GetType()))
            {
                Statuses[status.GetType()] = status;
            }
        }

        public T GetStatus<T>() where T : class, IStatus
        {
            if (Statuses.TryGetValue(typeof(T), out IStatus status))
            {
                return status as T;
            }
            return EmptyStatus.Instance as T;
        }
        public override string ToString()
        {
            return $"{GetType().Name} : {string.Join(", ", Statuses.Select(kvp => $"{kvp.Value}"))}";
        }
    }
}
