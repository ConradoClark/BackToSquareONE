using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class Room : BaseGameObject
{
    public bool Active { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; }

    [field: SerializeField]
    public Vector2Int RoomPos { get; private set; }

    [field: SerializeField]
    public CinemachineVirtualCamera VirtualCamera { get; private set; }

    private RoomManager _roomManager;

    public event Action OnActivation;
    public event Action OnDeactivation;

    protected override void OnAwake()
    {
        base.OnAwake();
        _roomManager = _roomManager.FromScene();

        _roomManager.AddRoom(this);
        Deactivate();
    }

    public void Activate()
    {
        Active = true;
        VirtualCamera.enabled = true;
        OnActivation?.Invoke();
    }

    public void Deactivate()
    {
        Active = false;
        VirtualCamera.enabled = false;
        OnDeactivation?.Invoke();
    }
}
