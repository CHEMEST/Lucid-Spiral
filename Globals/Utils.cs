using Godot;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Managers;
using LucidSpiral.Managers.ManagerThings;
using LucidSpiral.Managers.ManagerUtils;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using LucidSpiral.StatusesAndEffects.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Globals
{
#nullable enable
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
        public static T? FindManager<T>(Node root) where T : class, IManager 
        {
            ManagerHub hub = root.GetNodeOrNull<ManagerHub>("ManagerHub");
            if (hub == null) { return null; }
            return hub.GetManager<T>();
        }
        public static MovementPattern? GetActiveMovementPattern(Node root)
        {
            return FindManager<MovementManager>(root)?.GetActiveBehavior() as MovementPattern;
        }
        public static void SetState(Node root, State state) 
        {
            StateManager? stateManager = FindManager<StateManager>(root);
            if (stateManager == null) return;
            stateManager.ActiveState = state;
        }
        public static T? FindStatus<T>(Node root) where T : class, IStatus
        {
            StatusManager? manager = FindManager<StatusManager>(root);
            T? status = manager?.GetStatus<T>();
            return status;
        }
        public static CollisionSet? FindCollisionSet(Node root, CollisionType type)
        {
            return FindManager<CollisionManager>(root)?.GetCollisionSet(type);
        }
        // this function makes me happy
        public static void ProcessCollisions(Node root, CollisionType rootCollisionType, CollisionType targetCollisionType, Action<CollisionSet> onCollision)
        {
            CollisionSet? nodeCollisionSet = FindCollisionSet(root, rootCollisionType);
            if (nodeCollisionSet == null) return;
            if (nodeCollisionSet.Type == CollisionType.Empty)
            {
                throw new NotSupportedException($"No valid {rootCollisionType} collision found for the given node.");
            }

            List<CollisionSet> overlappingCollisions = nodeCollisionSet.GetOverlappingCollisionSets(targetCollisionType);
            foreach (CollisionSet collision in overlappingCollisions)
            {
                onCollision(collision);
            }
        }

    }
}
