﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.MapGeneration.Generator
{
    
    internal partial class Generator : Node2D
    {
        [Export] public int MaxRooms = 10;
        [Export] public int RoomSize = 10;
        private int gridSize;

        private Random random;
        private TileMapLayer corridoorTileMap;
        private TileMapLayer roomTileMap;

        private Dictionary<Vector2I, bool> map = new(); // this is a dict bc I'll make it a map later. the bool will be changed to an enum to represent different things
        private List<Vector2I> roomPositions = new();
        private List<(Vector2I, Vector2I)> roomConnections = new();

        public override void _Ready()
        {
            gridSize = (int)Mathf.Sqrt(MaxRooms * RoomSize);
            roomTileMap = GetNode<TileMapLayer>("RoomTileMap");
            corridoorTileMap = GetNode<TileMapLayer>("CorridoorTileMap");
            random = Global.Random;

            GenerateRoomsOnMap();
            GenerateRoomPositions();
            GenerateConnections();

            PlaceCorridors();
            PlaceRooms();

            Display();
        }

        private void GenerateRoomPositions()
        {
            foreach (Vector2I cord in map.Keys)
            {
                if (map[cord] == false) continue; // for later checks
                roomPositions.Add( new Vector2I(
                    cord.X * RoomSize,
                    cord.Y * RoomSize
                    ));
            }
        }

        private void Display()
        {
            DisplayRooms();
            GD.Print("-----");
            DisplayConnections();
        }

        private void DisplayRooms()
        {
            foreach (Vector2I cord in map.Keys) 
            {
                GD.Print(cord + ": " + map[cord]);
            }
        }
        private void DisplayConnections()
        {
            foreach ((Vector2I, Vector2I) pair in roomConnections)
            {
                GD.Print("[" + pair.Item1 + ", " + pair.Item2 + "]");
            }
        }
        /// <summary>
        /// Marks true at a certain location in dictionary if that slot is available for a room
        /// Very greedy, will make a cluster of rooms with some branches, add random stops and minRooms
        /// 
        /// </summary>
        private void GenerateRoomsOnMap()
        {
            map.Clear();
            Queue<Vector2I> queue = new(); // Vector2I is js pair<int, int>

            // initial cursor at middle
            Vector2I start = new(gridSize / 2, gridSize / 2);
            queue.Enqueue(start);
            map[start] = true;

            // while can place rooms
            while (queue.Count > 0 && map.Count < MaxRooms)
            {
                // moving cursor to next
                Vector2I current = queue.Dequeue();
                List<Vector2I> directions = new List<Vector2I> { Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right };
                // neat shuffle trick: https://stackoverflow.com/questions/273313/randomize-a-listt
                directions = directions.OrderBy(_ => random.Next()).ToList();
                

                foreach (Vector2I dir in directions)
                {
                    Vector2I newPos = current + dir;

                    if (map.Count >= MaxRooms) { GD.Print("Max Reached");  return; }
                    if (map.ContainsKey(newPos)) { GD.Print("Bad Choice"); continue; }

                    if (newPos.X >= 0 && newPos.X < gridSize && newPos.Y >= 0 && newPos.Y < gridSize) // checking bounds
                    {
                        map[newPos] = true; // mark a new room
                        queue.Enqueue(newPos); // add to queue for advancing
                    }
                }
            }
        }
        /// <summary>
        /// Generates a list of pairs (edges) as possible connections and cuts it down as much as possible (MST)
        /// </summary>
        private void GenerateConnections()
        {
            List<(Vector2I, Vector2I)> edges = new();

            // Create a fully connected graph
            for (int i = 0; i < roomPositions.Count; i++)
            {
                for (int j = i + 1; j < roomPositions.Count; j++)
                {
                    edges.Add((roomPositions[i], roomPositions[j]));
                }
            }

            // Sort by distance
            edges.Sort((a, b) => a.Item1.DistanceSquaredTo(a.Item2).CompareTo(b.Item1.DistanceSquaredTo(b.Item2)));

            // Kruskal’s Algorithm with Path Compression; not that I know how it works
            var parent = new Dictionary<Vector2I, Vector2I>();
            Vector2I Find(Vector2I v)
            {
                if (parent[v] != v)
                    parent[v] = Find(parent[v]);
                return parent[v];
            }

            foreach (var room in roomPositions) parent[room] = room;

            foreach (var (roomA, roomB) in edges)
            {
                var rootA = Find(roomA);
                var rootB = Find(roomB);
                if (rootA != rootB)
                {
                    roomConnections.Add((roomA, roomB));
                    parent[rootA] = rootB;
                }
            }
        }

        private void PlaceCorridors()
        {
            foreach (var (roomA, roomB) in roomConnections)
            {
                DrawLine(roomA, roomB);
            }
        }

        // Bresenham’s Line Algorithm to draw corridors
        // I watched a vid on how/why this works but that doesn't mean I can explain it lmao
        private void DrawLine(Vector2I start, Vector2I end)
        {
            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int err = dx - dy;

            Vector2I current = start;
            while (current != end)
            {
                corridoorTileMap.SetCell(current, 0, new Vector2I(0, 0)); // Place floor tile
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    current.X += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    current.Y += sy;
                }
            }
            corridoorTileMap.SetCell(end, 0, new Vector2I(0, 0)); // Ensure last tile is placed
        }

        private void PlaceRooms()
        {
            foreach (var roomPos in roomPositions)
            {
                roomTileMap.SetCell(roomPos, 0, new Vector2I(1, 1));
            }
        }

    }
}
