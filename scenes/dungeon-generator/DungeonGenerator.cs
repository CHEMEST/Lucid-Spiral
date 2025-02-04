using Godot;
using System.Collections.Generic;

public partial class DungeonGenerator : Node
{
    [Export] public string RoomFolder = "res://Rooms";
    [Export] public PackedScene StartRoom;
    [Export] public int MaxRooms = 10;

    private List<PackedScene> roomScenes = new List<PackedScene>();
    private List<RoomInstance> placedRooms = new List<RoomInstance>();
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        LoadRoomScenes();
        GenerateDungeon();
        
    }

    private void LoadRoomScenes()
    {
        DirAccess dir = DirAccess.Open(RoomFolder);
        if (dir != null)
        {
            string[] files = dir.GetFiles();
            foreach (string fileName in files)
            {
                if (fileName.EndsWith(".tscn"))
                {
                    PackedScene roomScene = GD.Load<PackedScene>($"{RoomFolder}/{fileName}");
                    if (roomScene != null)
                    {
                        roomScenes.Add(roomScene);
                    }
                }
            }
        }
    }

    private void GenerateDungeon()
    {
        if (StartRoom == null) return;

        RoomInstance start = PlaceRoom(StartRoom, Vector2.Zero);
        if (start == null) return;

        List<Node2D> openExits = new List<Node2D>(start.Exits);

        for (int i = 1; i < MaxRooms; i++)
        {
            if (openExits.Count == 0) break;

            Node2D exitToUse = openExits[rng.RandiRange(0, openExits.Count - 1)];
            openExits.Remove(exitToUse);

            if (roomScenes.Count == 0) break;

            PackedScene roomScene = roomScenes[rng.RandiRange(0, roomScenes.Count - 1)];
            RoomInstance newRoom = PlaceRoom(roomScene, exitToUse.GlobalTransform.Origin, exitToUse.RotationDegrees);

            if (newRoom != null)
            {
                openExits.AddRange(newRoom.Exits);
            }
        }
    }

    private RoomInstance PlaceRoom(PackedScene scene, Vector2 position, float rotation = 0)
    {
        RoomInstance room = scene.Instantiate<RoomInstance>();
        room.Position = position;
        room.RotationDegrees = rotation;
        AddChild(room);

        if (!Overlaps(room))
        {
            placedRooms.Add(room);
            return room;
        }

        room.QueueFree();
        return null;
    }

    private bool Overlaps(RoomInstance room)
    {
        foreach (var placed in placedRooms)
        {
            if (placed.Overlaps(room))
                return true;
        }
        return false;
    }
}
