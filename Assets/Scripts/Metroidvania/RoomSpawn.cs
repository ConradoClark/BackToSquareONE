using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RoomSpawn : RoomObject
{
    [field:SerializeField]
    public int SpawnId { get; private set; }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.forward, Vector3.one);
        Handles.Label(transform.position, SpawnId.ToString());
    }
}
