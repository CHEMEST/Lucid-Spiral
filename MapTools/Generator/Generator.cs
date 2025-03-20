using Godot;
using LucidSpiral.MapTools.MapUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucidSpiral.MapGeneration.Generator
{
    
    internal partial class Generator : Node2D
    {
        [Export] public int MaxRooms = 10;
        [Export] public int RoomSize = 25;
        private int gridSize;

        private Random random;
        private TileMapLayer corridoorTileMap;
        private string roomsFolderPath = "res://MapTools/Rooms/";

        private List<EntryPoint> entries = new();
        private Dictionary<Vector2I, Room> map = new();
        private Room activeRoom;
        private Vector2I mapCursor;
        private EntryPoint entryCursor;

        public override void _Ready()
        {
            gridSize = (int)Mathf.Sqrt(MaxRooms * RoomSize);
            corridoorTileMap = GetNode<TileMapLayer>("CorridorTileMap");
            random = Global.Random;

            GenerateRoomsOnMap();

        }

        private void GenerateRoomsOnMap()
        {
            map.Clear();

            activeRoom = LoadRandomRoom(); // pick a random Room from folder and instance it (do not add child or place pos)

            mapCursor = new(gridSize / 2, gridSize / 2);
            EnrollActiveRoom();

            // while can place rooms
            while (entries.Count > 0 && map.Count < MaxRooms)
            {
                entryCursor = entries[0];
                Vector2I dir = entryCursor.Dir;

                RedirectMapCursor();

                mapCursor += dir;
                if (map.ContainsKey(mapCursor))
                {
                    EntryPoint? oppositeEntry = FindOppositeEntryPointInRoom(map[mapCursor], entryCursor);
                    if (oppositeEntry == null) continue;
                    AddConnection(entryCursor, oppositeEntry);
                    if (!entries.Contains(oppositeEntry)) throw new Exception("room overlap is happening");

                }
                else
                {
                    Tuple<Room, EntryPoint> roomAndEntry = LoadRoomWithOppositeEntry(entryCursor);
                    // move cursors to next spot
                    activeRoom = roomAndEntry.Item1;

                    EnrollActiveRoom();

                    AddConnection(entryCursor, roomAndEntry.Item2);
                }
            }
        }
        #nullable enable
        private EntryPoint? FindOppositeEntryPointInRoom(Room room, EntryPoint entryCursor)
        {
            throw new NotImplementedException();
        }

        private void RedirectMapCursor()
        {
            activeRoom = entryCursor.Room;
            foreach (Vector2I key in map.Keys)
            {
                if (map[key] == activeRoom)
                {
                    mapCursor = key;
                    return;
                }
            }
            throw new Exception("room being looked for is not in map");
        }

        private void EnrollActiveRoom()
        {
            map[mapCursor] = activeRoom;
            foreach (EntryPoint entry in activeRoom.Entries)
            {
                entries.Add(entry);
            }
        }

        private void AddConnection(EntryPoint entryA, EntryPoint entryB)
        {
            entries.Remove(entryA);
            entries.Remove(entryB);
            throw new NotImplementedException();
        }

        /// <param name="entryCursor"></param>
        /// <returns>a Room with an opposite entry and the opposite EntryPoint</returns>
        private Tuple<Room, EntryPoint> LoadRoomWithOppositeEntry(EntryPoint entryCursor)
        {
            throw new NotImplementedException();
        }

        private Room LoadRandomRoom()
        {
            throw new NotImplementedException();
        }

        //private void SetPlayerPosition()
        //{
        //    Vector2 tileSize = corridoorTileMap.TileSet.TileSize;
        //    Vector2 startPosition = roomPositions[0] * tileSize;
        //    Global.Player.GlobalPosition = startPosition;

        //}
        //private Vector2I GetGlobalRoomPos(int index)
        //{
        //    Vector2I tileSize = corridoorTileMap.TileSet.TileSize;
        //    return roomPositions[index] * tileSize;
        //}
        //private void GenerateRoomPositions()
        //{
        //    foreach (Vector2I cord in mapOld.Keys)
        //    {
        //        if (mapOld[cord] == false) continue; // for later checks
        //        roomPositions.Add( new Vector2I(
        //            cord.X * RoomSize,
        //            cord.Y * RoomSize
        //            ));
        //    }
        //}
        //private void Display()
        //{
        //    DisplayRooms();
        //    GD.Print("-----");
        //    DisplayConnections();
        //}
        //private void DisplayRooms()
        //{
        //    foreach (Vector2I cord in mapOld.Keys) 
        //    {
        //        GD.Print(cord + ": " + mapOld[cord]);
        //    }
        //}
        //private void DisplayConnections()
        //{
        //    foreach ((Vector2I, Vector2I) pair in roomConnections)
        //    {
        //        GD.Print("[" + pair.Item1 + ", " + pair.Item2 + "]");
        //    }
        //}
        /// <summary>
        /// Marks true at a certain location in dictionary if that slot is available for a room
        /// Very greedy, will make a cluster of rooms with some branches, add random stops and minRooms
        /// 
        /// </summary>
        //private void GenerateRoomsOnMapOld()
        //{
        //    mapOld.Clear();
        //    Queue<Vector2I> queue = new(); // Vector2I is js pair<int, int>

        //    // initial cursor at middle
        //    Vector2I start = new(gridSize / 2, gridSize / 2);
        //    queue.Enqueue(start);
        //    mapOld[start] = true;

        //    // while can place rooms
        //    while (queue.Count > 0 && mapOld.Count < MaxRooms)
        //    {
        //        // moving cursor to next
        //        Vector2I current = queue.Dequeue();
        //        List<Vector2I> directions = new List<Vector2I> { Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right };
        //        // neat shuffle trick: https://stackoverflow.com/questions/273313/randomize-a-listt
        //        directions = directions.OrderBy(_ => random.Next()).ToList();
                

        //        foreach (Vector2I dir in directions)
        //        {
        //            Vector2I newPos = current + dir;

        //            if (mapOld.Count >= MaxRooms) { GD.Print("Max Reached");  return; }
        //            if (mapOld.ContainsKey(newPos)) { GD.Print("Bad Choice"); continue; }

        //            if (newPos.X >= 0 && newPos.X < gridSize && newPos.Y >= 0 && newPos.Y < gridSize) // checking bounds
        //            {
        //                mapOld[newPos] = true; // mark a new room
        //                queue.Enqueue(newPos); // add to queue for advancing
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// Generates a list of pairs (edges) as possible connections and cuts it down as much as possible (MST)
        ///// </summary>
        //private void GenerateConnections()
        //{
        //    List<(Vector2I, Vector2I)> edges = new();

        //    // Create a fully connected graph
        //    for (int i = 0; i < roomPositions.Count; i++)
        //    {
        //        for (int j = i + 1; j < roomPositions.Count; j++)
        //        {
        //            edges.Add((roomPositions[i], roomPositions[j]));
        //        }
        //    }

        //    // Sort by distance
        //    edges.Sort((a, b) => a.Item1.DistanceSquaredTo(a.Item2).CompareTo(b.Item1.DistanceSquaredTo(b.Item2)));

        //    // Kruskal’s Algorithm with Path Compression; not that I know how it works
        //    var parent = new Dictionary<Vector2I, Vector2I>();
        //    Vector2I Find(Vector2I v)
        //    {
        //        if (parent[v] != v)
        //            parent[v] = Find(parent[v]);
        //        return parent[v];
        //    }

        //    foreach (var room in roomPositions) parent[room] = room;

        //    foreach (var (roomA, roomB) in edges)
        //    {
        //        var rootA = Find(roomA);
        //        var rootB = Find(roomB);
        //        if (rootA != rootB)
        //        {
        //            roomConnections.Add((roomA, roomB));
        //            parent[rootA] = rootB;
        //        }
        //    }
        //}
        //private void PlaceCorridors()
        //{
        //    foreach (var (roomA, roomB) in roomConnections)
        //    {
        //        DrawLine(roomA, roomB);
        //    }
        //}

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
        //CGPT
        private List<string> GetSceneFiles(string path)
        {
            List<string> sceneFiles = new List<string>();
            DirAccess dir = DirAccess.Open(path);

            if (dir != null)
            {
                dir.ListDirBegin();
                string fileName = dir.GetNext();

                while (!string.IsNullOrEmpty(fileName))
                {
                    if (fileName.EndsWith(".tscn")) // Ensure it's a Godot scene
                    {
                        sceneFiles.Add(path + fileName);
                    }
                    fileName = dir.GetNext();
                }
                dir.ListDirEnd();
            }
            else
            {
                GD.PrintErr($"Failed to open directory: {path}");
            }

            return sceneFiles;
        }
        // part CGPT
        //private void LoadAndSpawnScene(string scenePath, int index)
        //{
        //    PackedScene scene = (PackedScene)ResourceLoader.Load(scenePath);
        //    if (scene != null)
        //    {
        //        Node2D instance = scene.Instantiate() as Node2D;
        //        instance.GlobalPosition = GetGlobalRoomPos(index);
        //        AddChild(instance);
        //        GD.Print($"Loaded scene: {scenePath}");
        //    }
        //    else
        //    {
        //        GD.PrintErr($"Failed to load scene: {scenePath}");
        //    }
        //}
        //private void PlaceRooms()
        //{
        //    for (int i = 0; i < roomPositions.Count; i++)
        //    {
        //        List<string> scenePaths = GetSceneFiles(RoomsFolderPath);
        //        if (scenePaths.Count > 0)
        //        {
        //            string randomScene = scenePaths[(int)(Global.Random.NextDouble() * scenePaths.Count)];
        //            LoadAndSpawnScene(randomScene, i);
        //        }
        //    }
        //}
    }
}
