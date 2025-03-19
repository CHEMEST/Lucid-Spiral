using Godot;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;
using LucidSpiral.Behaviors.Effects.EffectUtils;
using LucidSpiral.Entities.Creatures;
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
    partial class Comms : Node
    {

        /// <typeparam name="T"></typeparam>
        /// <param name="root"> the owner of the tree you are searching (ie. the root)</param>
        /// <returns></returns>
        public static T? FindManager<T>(Node root) where T : class, IManager 
        {
            ManagerHub hub = root.GetNodeOrNull<ManagerHub>("ManagerHub");
            return hub.GetManager<T>();
        }
        public static Entity? FindEntityCarrying(Node child)
        {
            Node current = child;

            while (current != null)
            {
                if (current is Entity entity)
                {
                    return entity;
                }
                current = current.GetParent();
            }

            return null;
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
        public static void HitboxAddIgnore(Node root, Node ignoring)
        {
            FindManager<CollisionManager>(root)?.GetCollisionSet(CollisionType.Hitbox)?.Ignoring.Add(ignoring);
        }
        public static void ProcessCollisions(Node root, CollisionType rootCollisionType, CollisionType targetCollisionType, Action<CollisionSet> onCollision)
        {
            CollisionSet? nodeCollisionSet = FindCollisionSet(root, rootCollisionType);
            if (nodeCollisionSet == null) return;
            if (nodeCollisionSet.Type == CollisionType.Empty)
            {
                throw new NotSupportedException($"No valid {rootCollisionType} collision found for the given node.");
            }

            List<CollisionSet> overlappingCollisions = nodeCollisionSet.GetOverlappingCollisionSets(targetCollisionType);
            // filtering all parents
            //for (int i = 0; i < overlappingCollisions.Count; i++)
            //{
            //    if (root.IsAncestorOf(overlappingCollisions[i].GetOwner())) overlappingCollisions.RemoveAt(i);
            //}
            overlappingCollisions.ForEach(onCollision);
        }

        public static void ApplyEffect(Node root, Effect effect)
        {
            EffectManager? manager = FindManager<EffectManager>(root);
            if (manager == null)
            {
                EffectManager newManager = new EffectManager();
                root.GetNodeOrNull<ManagerHub>("ManagerHub")?.AddChild(newManager);
                manager = newManager;
            }

            manager.AddChild(effect);
        }

    }
}
