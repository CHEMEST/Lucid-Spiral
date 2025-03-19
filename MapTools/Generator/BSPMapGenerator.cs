using Godot;
using LucidSpiral.MapTools.MapUtils;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BSPMapGenerator : Node2D
{
    [Export] public int MapWidth = 8024;
    [Export] public int MapHeight = 8024;
    [Export] public int MinRoomSize = 512;
    [Export] public int MaxRoomSize = 1024;
    [Export] public int MinRooms = 6;
    [Export] public int MaxRooms = 15;
    [Export] public string RoomsFolder = "res://MapTools//Rooms";
    [Export] public Node2D roomNode;
    private TileMapLayer corridorTileMap;
    Vector2I tileSize;

    private List<Rect2> rooms = new List<Rect2>();
    private List<PackedScene> roomScenes = new();
    private List<EntryPoint> availableEntries = new();
    private List<(EntryPoint, EntryPoint)> connections = new();
    private AStarGrid2D astarGrid;
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
    private void ConnectEntryPoints()
    {
        connections.Clear();

        if (availableEntries.Count < 2)
        {
            GD.PrintErr("Not enough entry points to create connections!");
            return;
        }

        List<(EntryPoint, EntryPoint, float)> allEdges = new();

        for (int i = 0; i < availableEntries.Count; i++)
        {
            for (int j = i + 1; j < availableEntries.Count; j++)
            {
                EntryPoint entryA = availableEntries[i];
                EntryPoint entryB = availableEntries[j];
                float distance = entryA.GlobalPosition.DistanceTo(entryB.GlobalPosition);
                Vector2 dir = entryA.GlobalPosition.DirectionTo(entryB.GlobalPosition);


                if (entryA.Room == entryB.Room || entryA.GetOppositeDirection() != entryB.Direction || dir.Dot(entryA.Dir) <= 0)
                    continue;


                allEdges.Add((entryA, entryB, distance));
            }
        }

        allEdges.Sort((a, b) => a.Item3.CompareTo(b.Item3));

        HashSet<EntryPoint> connectedEntries = new();

        foreach (var (entryA, entryB, _) in allEdges)
        {
            if (!connectedEntries.Contains(entryA) && !connectedEntries.Contains(entryB))
            {
                connections.Add((entryA, entryB));
                connectedEntries.Add(entryA);
                connectedEntries.Add(entryB);
            }

            if (connectedEntries.Count == availableEntries.Count)
                break;
        }
    }
    //             corridorTileMap.SetCell(tile, 0, new Vector2I(0, 0));

    private void BuildBasePaths()
    {
        foreach ((EntryPoint start, EntryPoint end) in connections)
        {
            Vector2 startTile = corridorTileMap.LocalToMap(start.GlobalPosition);
            Vector2 endTile = corridorTileMap.LocalToMap(end.GlobalPosition);
            // A star is way too slow rn
            //InitializeAStarGrid(MapWidth, MapHeight, 1); // Grid size: 100x100, Cell size: 1 unit
            //DrawThickAStarPath(startTile, endTile, 5);
            DrawThickLPath(startTile, endTile, 5);
        }
    }

    private void InitializeAStarGrid(int width, int height, int cellSize)
    {
        astarGrid = new AStarGrid2D();
        astarGrid.Region = new Rect2I(0, 0, width, height); // Define grid bounds
        astarGrid.CellSize = new Vector2I(cellSize, cellSize);
        astarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never; // Only allow cardinal directions
        astarGrid.DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan; // Better for grid movement

        // Mark all cells as walkable initially
        astarGrid.Update();
    }
    private void DrawThickAStarPath(Vector2 start, Vector2 end, int thickness)
    {
        List<Vector2I> path = astarGrid.GetIdPath((Vector2I)start, (Vector2I)end).ToList();

        if (path.Count > 0)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                DrawThickBresenhamLine(path[i], path[i + 1], thickness);
            }
        }
    }


    private void DrawThickBresenhamLine(Vector2 start, Vector2 end, int thickness)
    {
        Vector2 perpendicular = (end - start).Normalized().Orthogonal(); // Get a perpendicular vector
        for (int i = -thickness / 2; i <= thickness / 2; i++)
        {
            Vector2 offsetStart = start + perpendicular * i;
            Vector2 offsetEnd = end + perpendicular * i;

            DrawBresenhamLine(offsetStart, offsetEnd);
        }
    }

    private void DrawBresenhamLine(Vector2 start, Vector2 end)
    {
        List<Vector2> points = BresenhamLine((int)start.X, (int)start.Y, (int)end.X, (int)end.Y);
        foreach (Vector2I point in points)
        {
            corridorTileMap.SetCell(point, 1, new Vector2I(0, 0));
        }
    }
    private void DrawThickLPath(Vector2 start, Vector2 end, int thickness)
    {
        Vector2 midPoint = new Vector2(end.X, start.Y); // Horizontal first, then vertical
        DrawThickBresenhamLine(start, midPoint, thickness);
        DrawThickBresenhamLine(midPoint, end, thickness);
    }

    private void DrawLPath(Vector2 start, Vector2 end)
    {
        Vector2 midPoint = new Vector2(end.X, start.Y); // First go horizontally, then vertically
        DrawBresenhamLine(start, midPoint);
        DrawBresenhamLine(midPoint, end);
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
