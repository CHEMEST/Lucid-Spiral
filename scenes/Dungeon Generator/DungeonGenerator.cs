using Godot;
using System.Collections.Generic;

public partial class DungeonGenerator : Node
{
    [Export] public string RoomFolder = "res://Rooms";
    [Export] public PackedScene StartRoom;
    [Export] public int MaxRooms = 10;

    private List<PackedScene> roomScenes;
    List<Area2D> colliders = new List<Area2D>();
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        roomScenes = LoadRooms(RoomFolder);
        
    }
    // this does not modify roomScenes directly so that the same function can be used for other rooms later (boss, start, specials)
    private List<PackedScene> LoadRooms(string folderPath)
    {
        string[] files = DirAccess.GetFilesAt(folderPath); // Get all room scenes in the folder
        List<PackedScene> rooms = new List<PackedScene>();

        foreach (string file in files)
        {
            if (file.EndsWith(".tscn")) // Load only Godot scenes
            {
                PackedScene roomScene = (PackedScene)ResourceLoader.Load(folderPath + "/" + file);
                rooms.Add(roomScene);
            }
        }
        return rooms;
    }



    private RoomInstance PlaceRoom(PackedScene scene, Vector2 position, float rotation = 0)
    {
        RoomInstance room = scene.Instantiate<RoomInstance>();
        room.Position = position;
        room.RotationDegrees = rotation;
        AddChild(room);

        if (room.Overlaps(colliders))
        {
            room.QueueFree();
            return null;
        }

        colliders.Add(room.getRoomCollider());
        return room;
    }

}
