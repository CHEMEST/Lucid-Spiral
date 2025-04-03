using Godot;
using LucidSpiral.Managers.ManagerThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.Managers
{
    [GlobalClass]
    internal partial class SpriteManager : Node2D, IManager
    {
        public List<Sprite2D> GetSprites()
        {
            List<Sprite2D> list = new();
            foreach (Node child in GetChildren())
            {
                if (child is Sprite2D sprite)
                {
                    list.Add(sprite);
                }
            }
            return list;
        }
    }
}
