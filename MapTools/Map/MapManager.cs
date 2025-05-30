using Godot;
using LucidSpiral.Entities.Creatures;
using LucidSpiral.Globals;
using LucidSpiral.MapTools.MapUtils;
using System;
using System.Collections.Generic;

public partial class MapManager : Node2D
{
    private Dictionary<Vector2I, Room> rooms = new();
    private Vector2I cursor;
    private List<PackedScene> availableRoomScenes = new();
    [Export] private string roomFolder = "res://MapTools/Rooms/";
    [Export] private Node2D activeRoomParent;
    [Export] private Node2D organizationParent;

    private Dictionary<Direction, List<PackedScene>> roomsByDirection = new()
    {
        { Direction.North, new List<PackedScene>() },
        { Direction.South, new List<PackedScene>() },
        { Direction.East, new List<PackedScene>() },
        { Direction.West, new List<PackedScene>() }
    };
    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(DeferredSetup));
    }

    private void DeferredSetup()
    {
        loadRooms();
        organizeRoomsByDirection();

        // Debug print
        foreach (var entry in roomsByDirection)
        {
            GD.Print(entry.Key + ": ");
            foreach (var scene in entry.Value)
            {
                GD.Print("  " + scene);
            }
        }

        cursor = Vector2I.Zero;
        Room room = RandomRoom();
        rooms[cursor] = room;

        if (activeRoomParent != null)
        {
            activeRoomParent.AddChild(room);
        }
        else
        {
            GD.PushWarning("activeRoomParent is null in DeferredSetup");
        }
        foreach (Node child in activeRoomParent.GetChildren())
        {
            if (child is Room roomChild)
            {
                GD.Print(child.Name);
            }
        }

        CallDeferred(nameof(InitializePlayer));
    }


    private void InitializePlayer()
    {
        Vector2 pos = rooms[cursor].GlobalPosition + (rooms[cursor].Size) / 2;
        GD.Print("Initialized Player at ", pos);
        Global.Player.GlobalPosition = pos;
        GD.Print("Player is at ", Global.Player.GlobalPosition);

        Room room = activeRoomParent.GetChild<Room>(0);
        GD.Print("Room is at", room.GlobalPosition + room.Size/2);
    }

    private void organizeRoomsByDirection()
    {
        foreach (PackedScene roomScene in availableRoomScenes)
        {
            Room room = roomScene.Instantiate<Room>();
            organizationParent.AddChild(room);

            foreach (EntryPoint entry in room.Entries)
            {
                roomsByDirection[entry.Direction].Add(roomScene);
            }
            room.QueueFree();
        }
    }

    public void loadRooms()
    {
        List<PackedScene> roomScenes = Utils.LoadScenesFromFolder(roomFolder);
        foreach (PackedScene roomScene in roomScenes)
        {
            availableRoomScenes.Add(roomScene);
        }
    }

    private Room RandomRoom()
    {
        Room room = availableRoomScenes[Global.Random.Next() % availableRoomScenes.Count].Instantiate() as Room;
        if (room == null)
        {
            GD.PrintErr("No Room to load");
        }
        return room;
    }
    private Room RandomRoomWithOppositeEntry(EntryPoint entry)
    {
        Direction dir = entry.GetOppositeDirection();
        GD.Print(dir);
        List<PackedScene> roomScenes = roomsByDirection[dir];
        if (roomScenes.Count == 0)
        {
            GD.PrintErr("No Room to load 1");
            return null;
        }
        Room room = roomScenes[Global.Random.Next() % roomScenes.Count].Instantiate() as Room;
        if (room == null)
        {
            GD.PrintErr("No Room to load 2");
        }
        return room;
    }


    public void SwitchRoom(EntryPoint entryPoint)
    {
        // save the current room
        rooms[cursor] = activeRoomParent.GetChild<Room>(0);
        activeRoomParent.RemoveChild(rooms[cursor]);

        // load new room
        cursor += entryPoint.Dir;
        if (rooms.ContainsKey(cursor))
        {
            activeRoomParent.AddChild(rooms[cursor]);
        }
        else
        {
            AddNewRoom(entryPoint);
        }
        MovePlayer(entryPoint);

        GD.Print(cursor);
    }

    private void MovePlayer(EntryPoint entryInital)
    {
        Direction dir = entryInital.GetOppositeDirection();
        EntryPoint oppositeEntry = null;
        foreach ( EntryPoint entry in rooms[cursor].Entries)
        {
            if (entry.Direction == dir)
            {
                oppositeEntry = entry;
                break;
            }
        }

        Player player = Global.Player;
        Vector2 pos;
        if (oppositeEntry == null) // move to center of room if no opposite entry
        {
            pos = rooms[cursor].Position + (rooms[cursor].Size) / 2;
        }
        else // move to an offset of the entry location
        {
            pos = oppositeEntry.GetChild<CollisionShape2D>(0).GlobalPosition + oppositeEntry.Dir * -50;
        }

        player.GlobalPosition = pos;

    }

    private void AddNewRoom(EntryPoint entryPoint)
    {
        Room room = RandomRoomWithOppositeEntry(entryPoint);
        rooms[cursor] = room;
        activeRoomParent.AddChild(room);
    }
}
