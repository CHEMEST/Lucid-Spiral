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
    partial class Utils : Node
    {
        public static T FindManager<T>(Node root) where T : class, IManager
        {
            return root.GetOwner().GetNodeOrNull<ManagerHub>("ManagerHub").GetManager<T>();
        }
    }
}
