using Godot;
using LucidSpiral.MapTools.MapUtils;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BSPMapGenerator : Node2D
{
    [Export] public int MapWidth = 8000;
    [Export] public int MapHeight = 8000;
    [Export] public int MinRoomSize = 600;
    [Export] public int MaxRoomSize = 1200;
    [Export] public int MinRooms = 5;
    [Export] public int MaxRooms = 15;
    [Export] public string RoomsFolder = "res://MapTools//Rooms";
    [Export] public Node2D roomNode;
    private TileMapLayer corridorTileMap;
    Vector2I tileSize;

    private List<Rect2> rooms = new List<Rect2>();
    private List<PackedScene> roomScenes = new List<PackedScene>();
    private List<EntryPoint> availableEntries = new List<EntryPoint>();
    private List<(EntryPoint, EntryPoint)> connections = new List<(EntryPoint, EntryPoint)>();
    private Random rand = Global.Random;

    public override void _Ready()
    {
        corridorTileMap = GetNode<TileMapLayer>("CorridoorTileMap");
        tileSize = corridorTileMap.TileSet.TileSize;
        LoadRoomScenes();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.Space)
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        rooms.Clear();
        availableEntries.Clear();
        connections.Clear();
        corridorTileMap.Clear();

        foreach (Node child in roomNode.GetChildren())
        {
            child.QueueFree();
        }

        GenerateBSP();
        GD.Print("Entries: ");
        foreach (EntryPoint entry in availableEntries)
        {
            GD.Print(entry);
        }
        ConnectEntryPoints();
        GD.Print("Connections: ");
        foreach ((EntryPoint, EntryPoint) connection in connections)
        {
            GD.Print(connection);
        }
        BuildPaths();
    }

    private void LoadRoomScenes()
    {
        DirAccess dir = DirAccess.Open(RoomsFolder);
        if (dir != null && dir.ListDirBegin() == Error.Ok)
        {
            string file;
            while ((file = dir.GetNext()) != "")
            {
                if (file.EndsWith(".tscn"))
                {
                    var scene = GD.Load<PackedScene>($"{RoomsFolder}/{file}");
                    if (scene != null)
                        roomScenes.Add(scene);
                }
            }
        }
    }

    private void GenerateBSP()
    {
        if (roomScenes.Count == 0)
        {
            GD.PrintErr("No room scenes found in the Rooms folder!");
            return;
        }

        List<Rect2> partitions = new() { };
        while (partitions.Count < MinRooms)
        {
            //GD.Print("Attempting Partitioning");
            partitions = PartitionSpace(new Rect2(0, 0, MapWidth, MapHeight), MinRooms, MaxRooms);
        }
        //GD.Print("Resulting Partitions: " + partitions.Count);
        foreach (Rect2 partition in partitions)
        {
            PlaceRoom(partition);
        }
    }

    private List<Rect2> PartitionSpace(Rect2 space, int minPartitions, int maxPartitions)
    {
        List<Rect2> partitions = new List<Rect2> { space };
        int numPartitions = rand.Next(minPartitions, maxPartitions + 1);
        //GD.Print("Starting BSP for " +  numPartitions + " partitions");

        while (partitions.Count > 0 && partitions.Count < numPartitions)
        {
            Rect2 partition = partitions[rand.Next(partitions.Count)];
            partitions.Remove(partition);

            bool splitVertically = rand.Next(2) == 0;
            if (partition.Size.X < MaxRoomSize * 2 || partition.Size.Y < MaxRoomSize * 2)
            {
                //GD.Print("Skipped partition");
                continue;
            }
            //GD.Print("Successful partition");

            if (splitVertically)
            {
                float splitX = rand.Next((int)partition.Position.X + MinRoomSize, (int)(partition.End.X - MinRoomSize));
                partitions.Add(new Rect2(partition.Position, new Vector2(splitX - partition.Position.X, partition.Size.Y)));
                partitions.Add(new Rect2(new Vector2(splitX, partition.Position.Y), new Vector2(partition.End.X - splitX, partition.Size.Y)));
            }
            else
            {
                float splitY = rand.Next((int)partition.Position.Y + MinRoomSize, (int)(partition.End.Y - MinRoomSize));
                partitions.Add(new Rect2(partition.Position, new Vector2(partition.Size.X, splitY - partition.Position.Y)));
                partitions.Add(new Rect2(new Vector2(partition.Position.X, splitY), new Vector2(partition.Size.X, partition.End.Y - splitY)));
            }
        }
        return partitions;
    }

    private void PlaceRoom(Rect2 partition)
    {
        PackedScene roomScene = roomScenes[rand.Next(roomScenes.Count)];
        Room roomInstance = roomScene.Instantiate<Room>();

        float roomWidth = rand.Next(MinRoomSize, (int)Mathf.Min(MaxRoomSize, partition.Size.X));
        float roomHeight = rand.Next(MinRoomSize, (int)Mathf.Min(MaxRoomSize, partition.Size.Y));

        float roomX = partition.Position.X + (partition.Size.X - roomWidth) / 2;
        float roomY = partition.Position.Y + (partition.Size.Y - roomHeight) / 2;

        Rect2 newRoom = new Rect2(roomX, roomY, roomWidth, roomHeight);
        //GD.Print("Attempting room at " + roomX + ", " + roomY);
        if (!rooms.Any(r => r.Intersects(newRoom)))
        {
            //GD.Print("Success");

            roomInstance.Position = new Vector2(roomX, roomY);
            roomNode.AddChild(roomInstance);
            rooms.Add(newRoom);

            foreach (EntryPoint entry in roomInstance.Entries)
            {
                availableEntries.Add(entry);
            }
        }
    }

    private void ConnectEntryPoints()
    {
        while (availableEntries.Count > 1)
        {
            EntryPoint entryA = availableEntries[0];
            EntryPoint closestEntry = null;
            float closestDist = float.MaxValue;

            foreach (EntryPoint entryB in availableEntries.Skip(1))
            {
                if (entryA.Room != entryB.Room)
                {
                    float dist = entryA.Position.DistanceTo(entryB.Position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestEntry = entryB;
                    }
                }
            }

            if (closestEntry != null)
            {
                connections.Add((entryA, closestEntry));
                availableEntries.Remove(entryA);
                availableEntries.Remove(closestEntry);
            }
            else
            {
                availableEntries.RemoveAt(0);
            }
        }
    }

    private void BuildPaths()
    {
        Vector2I tileToPlace = new Vector2I(0, 0);

        foreach ((EntryPoint start, EntryPoint end) in connections)
        {
            Vector2I startTile = corridorTileMap.LocalToMap(start.GlobalPosition);
            Vector2I endTile = corridorTileMap.LocalToMap(end.GlobalPosition);

            // Get the tiles forming the path
            List<Vector2I> path = GetBresenhamLine(startTile, endTile);

            // Place tiles along the path
            foreach (Vector2I tilePos in path)
            {
                corridorTileMap.SetCell(tilePos, 0, tileToPlace);
            }
        }
        //corridoorTileMap.SetCell(pos, 0, new Vector2I(0, 0));
    }

    private List<Vector2I> GetBresenhamLine(Vector2I start, Vector2I end)
    {
        List<Vector2I> line = new List<Vector2I>();

        int dx = Mathf.Abs(end.X - start.X);
        int dy = Mathf.Abs(end.Y - start.Y);
        int sx = start.X < end.X ? 1 : -1;
        int sy = start.Y < end.Y ? 1 : -1;
        int err = dx - dy;

        int x = start.X;
        int y = start.Y;

        while (true)
        {
            line.Add(new Vector2I(x, y));

            if (x == end.X && y == end.Y)
                break;

            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }

        return line;
    }

}
