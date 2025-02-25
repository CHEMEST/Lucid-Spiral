using Godot;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LucidSpiral.MovementPatterns;
using LucidSpiral.MovementPatterns.MovementPatternThings;
using System.Diagnostics;
using LucidSpiral.Actions.ActionUtils;
using LucidSpiral.Managers.ManagerUtils;
using LucidSpiral.Behaviors.BehaviorUtils;
using LucidSpiral.Behaviors.Collisions.CollisionUtils;


namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class CollisionManager : Node2D, IManager
    {
        public Dictionary<CollisionType, CollisionSet> CollisionSets { get; private set; } = new();

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is CollisionSet)
                {
                    CollisionSet collisionSet = child as CollisionSet;
                    CollisionSets.Add(collisionSet.Type, collisionSet);
                }
            }
        }
        /// <summary>
        /// Returns a CollisionSet of default type (Empty) if Type not found
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public CollisionSet GetCollisionSet(CollisionType type)
        {
            if (CollisionSets.TryGetValue(type, out CollisionSet collision))
            {
                return collision;
            }
            return new CollisionSet();
        }
    }
}
