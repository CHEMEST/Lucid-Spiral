using Godot;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers
{
    internal partial class EmptyManager : Node, IManager
    {
        public static EmptyManager Instance { get; } = new EmptyManager();
        
        private EmptyManager() { }

        public string GetManagerName() => "EmptyManager";
    }

}
