using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace LucidSpiral.Managers.ManagerThings
{
    [GlobalClass]
    internal partial class ManagerHub : Node2D
    {
        private Dictionary<Type, IManager> _managers = new();

        public override void _Ready()
        {
            // Register all IManager children in _managers
            foreach (Node child in GetChildren())
            {
                if (child is IManager manager)
                {
                    _managers[manager.GetType()] = manager;
                }
            }
        }
        // Searches through _managers for a T type object that inherits from IManager (ie. T is the parameter)
        #nullable enable
        public T? GetManager<T>() where T : class, IManager
        {
            if (_managers.TryGetValue(typeof(T), out IManager? manager))
            {
                return manager as T;
            }
            return null;
        }

        public void AddManager<T>(T manager) where T : Node, IManager
        {
            if (_managers.TryAdd(typeof(T), manager)) return;

            manager.QueueFree();
        }
    }

}
