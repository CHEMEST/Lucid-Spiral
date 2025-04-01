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
        [Export] public int MinRooms = 5;
        [Export] public int MaxRooms = 10;
        [Export] public int MaxRoomSize = 25;
        private int gridSize;

        private Random random;
        private TileMapLayer corridoorTileMap;
        private string roomsFolderPath = "res://MapTools/Rooms/";

        private List<EntryPoint> entries = new();
        private List<EntryPoint> unusableEntries = new();
        private Dictionary<Vector2I, Room> map = new();
        private Room activeRoom;
        private Vector2I mapCursor;
        private EntryPoint entryCursor;

        public override void _Ready()
        {
            gridSize = (int)Mathf.Sqrt(MaxRooms * MaxRoomSize);
            corridoorTileMap = GetNode<TileMapLayer>("CorridorTileMap");
            random = Global.Random;

            GenerateSuccessfulRoomMap();

        }
        private void GenerateSuccessfulRoomMap()
        {
            while (map.Count < MinRooms)
            {
                GenerateNewRoomMap();
            }
        }

        private void GenerateNewRoomMap()
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
                    if (oppositeEntry == null) continue; // make this add to a bad entries list
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
            if (map.Count < MinRooms)
            {
                // pull
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
        /// <summary>
        /// Instances (does not place) a random room from the available PackedScene Rooms
        /// </summary>
        /// <returns>A random Room instance</returns>
        /// <exception cref="NotImplementedException"></exception>
        private Room LoadRandomRoom()
        {
            throw new NotImplementedException();
        }

        //private void PlaceSceneAtTile(Vector2I tilePosition)
        //{
        //    // Convert tile coordinates to world position
        //    Vector2 worldPosition = TileMap.MapToLocal(tilePosition);

        //    // Instance the scene
        //    Node2D instance = ObjectScene.Instantiate<Node2D>();
        //    if (instance == null)
        //    {
        //        GD.PrintErr("Failed to instance scene!");
        //        return;
        //    }

        //    // Set position and add to the scene
        //    instance.Position = worldPosition;
        //    TileMap.AddChild(instance);
        //}

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
