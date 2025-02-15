using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers.ManagerThings
{

    /// <summary>
    /// Base interface for all managers
    /// </summary>
    public partial interface IManager
    {
        public string GetManagerName()
        {
            return GetType().ToString(); // Returns the name of the runtime IManager subclass
        }
    }

}
