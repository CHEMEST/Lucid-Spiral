using Godot;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Globals
{
    class Utils
    {
        public static T FindManager<T>(Node root) where T : class, IManager
        {
            return root.GetNodeOrNull<ManagerHub>("ManagerHub").GetManager<T>();
        }
    }
}
