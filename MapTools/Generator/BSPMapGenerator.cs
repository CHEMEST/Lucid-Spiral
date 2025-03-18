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
    private List<PackedScene> roomScenes = new();
    private List<EntryPoint> availableEntries = new();
    private List<(EntryPoint, EntryPoint)> connections = new();
    private Random rand = Global.Random;
    private bool x = false;

    public override void _Ready()
    {
        corridorTileMap = GetNode<TileMapLayer>("CorridorTileMap");
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
        BuildBasePaths();
        CarveWalkableTiles();

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
    // REVAMP everything after thus
    private void ConnectEntryPoints()
    {
        connections.Clear();

        if (availableEntries.Count < 2)
        {
            GD.PrintErr("Not enough entry points to create connections!");
            return;
        }

        // List to store all possible connections
        List<(EntryPoint, EntryPoint, float)> allEdges = new();

        // Compare each EntryPoint with every other EntryPoint
        for (int i = 0; i < availableEntries.Count; i++)
        {
            for (int j = i + 1; j < availableEntries.Count; j++)
            {
                EntryPoint entryA = availableEntries[i];
                EntryPoint entryB = availableEntries[j];

                // Prevent connecting an EntryPoint to itself or the same room
                if (entryA.Room == entryB.Room || entryA.GetOppositeDirection() != entryB.Direction)
                    continue;

                // Compute Euclidean distance between entry points
                float distance = entryA.GlobalPosition.DistanceTo(entryB.GlobalPosition);

                // Store the connection with distance
                allEdges.Add((entryA, entryB, distance));
            }
        }

        // Sort connections by shortest distance first
        allEdges.Sort((a, b) => a.Item3.CompareTo(b.Item3));

        // Track which entry points have been connected
        HashSet<EntryPoint> connectedEntries = new();

        // Go through the sorted edges and add the shortest connections
        foreach (var (entryA, entryB, _) in allEdges)
        {
            // If both entry points are not yet connected, add this connection
            if (!connectedEntries.Contains(entryA) && !connectedEntries.Contains(entryB))
            {
                connections.Add((entryA, entryB));
                connectedEntries.Add(entryA);
                connectedEntries.Add(entryB);
            }

            // Stop when all entries are connected
            if (connectedEntries.Count == availableEntries.Count)
                break;
        }
    }
    //             corridorTileMap.SetCell(tile, 0, new Vector2I(0, 0));

    private void BuildBasePaths()
    {
        // Example connections (start, end) as pairs of EntryPoint
        foreach ((EntryPoint start, EntryPoint end) in connections)
        {
            Vector2 startTile = corridorTileMap.LocalToMap(start.GlobalPosition);
            Vector2 endTile = corridorTileMap.LocalToMap(end.GlobalPosition);

            // Draw the thick L-shaped path between the two tiles
            DrawLPath(startTile, endTile);
        }
    }
    private void DrawLPath(Vector2 start, Vector2 end)
    {
        List<Vector2I> thickPath = new List<Vector2I>();

        Vector2I firstHorizontal = Vector2I.Zero;
        Vector2I lastHorizontal = Vector2I.Zero;
        Vector2I firstVertical = Vector2I.Zero;
        Vector2I lastVertical = Vector2I.Zero;

        bool firstHSet = false, firstVSet = false;

        for (int x = (int)start.X; x != (int)end.X; x += Math.Sign(end.X - start.X))
        {
            Vector2I tilePos = new Vector2I(x, (int)start.Y);
            thickPath.Add(tilePos);

            if (!firstHSet) { firstHorizontal = tilePos; firstHSet = true; }
            lastHorizontal = tilePos;

            thickPath.Add(new Vector2I(x + 2, (int)start.Y));
            thickPath.Add(new Vector2I(x - 2, (int)start.Y));
            thickPath.Add(new Vector2I(x, (int)start.Y + 2));
            thickPath.Add(new Vector2I(x, (int)start.Y - 2));
            thickPath.Add(new Vector2I(x + 1, (int)start.Y + 1));
            thickPath.Add(new Vector2I(x - 1, (int)start.Y + 1));
            thickPath.Add(new Vector2I(x + 1, (int)start.Y - 1));
            thickPath.Add(new Vector2I(x - 1, (int)start.Y - 1));
        }

        for (int y = (int)start.Y; y != (int)end.Y; y += Math.Sign(end.Y - start.Y))
        {
            Vector2I tilePos = new Vector2I((int)end.X, y);
            thickPath.Add(tilePos);

            if (!firstVSet) { firstVertical = tilePos; firstVSet = true; }
            lastVertical = tilePos;

            thickPath.Add(new Vector2I((int)end.X + 2, y));
            thickPath.Add(new Vector2I((int)end.X - 2, y));
            thickPath.Add(new Vector2I((int)end.X, y + 2));
            thickPath.Add(new Vector2I((int)end.X, y - 2));
            thickPath.Add(new Vector2I((int)end.X + 1, y + 1));
            thickPath.Add(new Vector2I((int)end.X - 1, y + 1));
            thickPath.Add(new Vector2I((int)end.X + 1, y - 1));
            thickPath.Add(new Vector2I((int)end.X - 1, y - 1));
        }

        foreach (Vector2I tile in thickPath)
        {
            corridorTileMap.SetCell(tile, 1, new Vector2I(0, 0));
        }

        corridorTileMap.SetCell(firstHorizontal, 0, new Vector2I(0, 0)); 
        corridorTileMap.SetCell(lastHorizontal, 0, new Vector2I(0, 0)); 
        corridorTileMap.SetCell(firstVertical, 0, new Vector2I(0, 0)); 
        corridorTileMap.SetCell(lastVertical, 0, new Vector2I(0, 0)); 
    }


    private void CarveWalkableTiles()
    {
        foreach (Vector2I tilePos in corridorTileMap.GetUsedCells())
        {
            int c = 0;
            foreach (Vector2I neighbor in corridorTileMap.GetSurroundingCells(tilePos))
            {
                if (corridorTileMap.GetCellSourceId(neighbor) != -1) c++;
            }
            if (c == 4)
            {
                corridorTileMap.SetCell(tilePos, 0, new Vector2I(0, 0));
            }
        }
    }

    private List<Vector2> BresenhamLine(int x0, int y0, int x1, int y1)
    {
        List<Vector2> points = new List<Vector2>();

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            points.Add(new Vector2(x0, y0));

            if (x0 == x1 && y0 == y1) break;

            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        return points;
    }

}
