using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Events;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEditor;
using UnityEngine;

public class RoomExit : RoomObject
{
    [field: SerializeField]
    public int ExitId { get; private set; }

    [field: SerializeField]
    public Vector2Int ExitDirection { get; private set; }

    private const float Offset = 1f;
    private Player _player;

    private IEventPublisher<RoomEvents, Room> _eventPublisher;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
        _eventPublisher = this.RegisterAsEventPublisher<RoomEvents, Room>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DefaultMachinery.AddBasicMachine(HandleExit());
    }

    private IEnumerable<IEnumerable<Action>> HandleExit()
    {
        while (ComponentEnabled)
        {
            var exited = false;
            if (ExitDirection == Vector2Int.right)
            {
                exited = _player.transform.position.x > transform.position.x + Offset;
            }
            else if (ExitDirection == Vector2Int.left)
            {
                exited = _player.transform.position.x < transform.position.x - Offset;
            }
            if (ExitDirection == Vector2Int.up)
            {
                exited = _player.transform.position.y > transform.position.y + Offset;
            }
            else if (ExitDirection == Vector2Int.down)
            {
                exited = _player.transform.position.y < transform.position.y - Offset;
            }

            if (exited)
            {
                _eventPublisher.PublishEvent(RoomEvents.OnRoomExit, Room);
                _eventPublisher.PublishEvent(RoomEvents.OnRoomEnter, RoomManager.GetNextRoom(Room, ExitDirection));
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Handles.Label(transform.position, ExitId.ToString());
    }


}
