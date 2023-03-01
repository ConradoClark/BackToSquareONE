using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class RoomManager : BaseGameObject
{
    [field:SerializeField]
    public CurrentRoom CurrentRoom { get; private set; }

    public IReadOnlyCollection<Room> Rooms => _rooms.Values.ToArray();
    private Dictionary<Vector2Int, Room> _rooms;

    protected override void OnAwake()
    {
        base.OnAwake();
        _rooms = new Dictionary<Vector2Int, Room>();
    }

    public void AddRoom(Room room)
    {
        _rooms ??= new Dictionary<Vector2Int, Room>();
        _rooms[room.RoomPos] = room;
    }

    public Room GetCurrentRoom()
    {
        return _rooms[CurrentRoom.CurrentRoomPos];
    }

    public Room GetNextRoom(Room room, Vector2Int direction)
    {
        var nextRoomPos = room.RoomPos + direction;
        if (!_rooms.ContainsKey(nextRoomPos)) throw new Exception($"Room {nextRoomPos} not found!");
        return _rooms[nextRoomPos];
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DefaultMachinery.AddBasicMachine(Init());
        this.ObserveEvent<RoomEvents,Room>(RoomEvents.OnRoomEnter, OnRoomEnter);
        this.ObserveEvent<RoomEvents, Room>(RoomEvents.OnRoomExit, OnRoomExit);
    }

    private IEnumerable<IEnumerable<Action>> Init()
    {
        yield return TimeYields.WaitOneFrameX;
        if (!_rooms.ContainsKey(CurrentRoom.CurrentRoomPos)) yield break;
        _rooms[CurrentRoom.CurrentRoomPos].Activate();
    }

    private void OnRoomExit(Room obj)
    {
        obj.Deactivate();
    }

    private void OnRoomEnter(Room obj)
    {
        obj.Activate();
        CurrentRoom.CurrentRoomPos = obj.RoomPos;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.StopObservingEvent<RoomEvents, Room>(RoomEvents.OnRoomEnter, OnRoomEnter);
        this.StopObservingEvent<RoomEvents, Room>(RoomEvents.OnRoomExit, OnRoomExit);
    }
}
