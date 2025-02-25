using Godot;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Globals
{
    partial class Utils : Node
    {
        // This can technically return a null but if it does that most likely
        // means you're searching the wrong thing or that thing's scene tree is messed up
        // since I doubt you would be looking for a manager in something without a ManagerHub...
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"> the owner of the tree you are searching (ie. the root)</param>
        /// <returns></returns>
        public static T FindManager<T>(Node root) where T : class, IManager
        {
            return root.GetNodeOrNull<ManagerHub>("ManagerHub").GetManager<T>();
        }
        public static T FindStatus<T>(Node root) where T : class, IStatus
        {
            return FindManager<StatusManager>(root).GetStatus<T>();
        }
        public static CollisionSet FindCollisionSet(Node root, CollisionType type)
        {
            return FindManager<CollisionManager>(root).GetCollisionSet(type);
        }
    }
}
