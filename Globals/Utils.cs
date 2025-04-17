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
    internal partial class Utils : Node
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

        // CGPT
        public static List<PackedScene> LoadScenesFromFolder(string folderPath)
        {
            DirAccess dir = DirAccess.Open(folderPath);
            if (dir == null)
            {
                GD.PrintErr("Failed to open directory: " + folderPath);
                return new();
            } 

            dir.ListDirBegin();
            string fileName = dir.GetNext();
            List<PackedScene> loadedScenes = new();

            while (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.EndsWith(".tscn") || fileName.EndsWith(".scn"))
                {
                    string filePath = folderPath + fileName;
                    PackedScene scene = ResourceLoader.Load<PackedScene>(filePath);
                    if (scene != null)
                    {
                        loadedScenes.Add(scene);
                        //GD.Print("Loaded scene: " + filePath);
                    }
                    else
                    {
                        GD.PrintErr("Failed to load scene: " + filePath);
                    }
                }
                fileName = dir.GetNext();
            }
            return loadedScenes;
        }
        public static void SetSubTreeEnabled(Node root, bool to)
        {
            var stack = new Stack<Node>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();

                current.SetProcess(to);
                current.SetPhysicsProcess(to);
                current.SetProcessInput(to);
                current.SetProcessUnhandledInput(to);
                current.SetProcessUnhandledKeyInput(to);

                foreach (Node child in current.GetChildren())
                {
                    stack.Push(child);
                }
            }
        }
        internal static void PauseGame()
        {
            Engine.Singleton.TimeScale = 0;
            SetSubTreeEnabled(Global.Main.World, false);
            SetSubTreeEnabled(Global.Main.GUI, false);
        }
        internal static void UnPauseGame()
        {
            Engine.Singleton.TimeScale = 1;
            SetSubTreeEnabled(Global.Main.World, true);
            SetSubTreeEnabled(Global.Main.GUI, true);
        }


    }
}
