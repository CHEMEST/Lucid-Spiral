using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.MapGeneration.Generator
{
    
    internal partial class Generator : Node2D
    {
        [Export] public int GridSize = 20;
        [Export] public int MaxRooms = 10;
        [Export] public int RoomSize = 3;

        private TileMapLayer roomTileMap;
        private Dictionary<Vector2I, bool> roomPositions = new();

        public override void _Ready()
        {
            roomTileMap = GetNode<TileMapLayer>("RoomTileMap");
            GenerateRoomList();
            DisplayRooms();
        }

        private void DisplayRooms()
        {
            foreach (Vector2I cord in roomPositions.Keys) 
            {
                GD.Print(cord + ": " + roomPositions[cord]);
            }
        }
        /// <summary>
        /// Marks true at a certain location in dictionary if that slot is available for a room
        /// Very greedy, will make a cluster of rooms with some branches, add random stops and minRooms
        /// 
        /// </summary>
        private void GenerateRoomList()
        {
            roomPositions.Clear();
            Queue<Vector2I> queue = new(); // Vector2I is js pair<int, int>

            // initial cursor at middle
            Vector2I start = new(GridSize / 2, GridSize / 2);
            queue.Enqueue(start);
            roomPositions[start] = true;

            // while can place rooms
            while (queue.Count > 0 && roomPositions.Count < MaxRooms)
            {
                // moving cursor to next
                Vector2I current = queue.Dequeue();
                List<Vector2I> directions = new List<Vector2I> { Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right };
                // should shuffle directions somehow to not be similar structures every time

                foreach (Vector2I dir in directions)
                {
                    Vector2I newPos = current + dir;

                    if (roomPositions.Count >= MaxRooms) return;
                    if (roomPositions.ContainsKey(newPos)) return;

                    if (newPos.X >= 0 && newPos.X < GridSize && newPos.Y >= 0 && newPos.Y < GridSize) // checking bounds
                    {
                        roomPositions[newPos] = true; // mark a new room
                        queue.Enqueue(newPos); // add to queue for advancing
                    }
                }
            }
        }
    }
}
