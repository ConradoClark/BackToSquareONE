using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentRoom", menuName = "ONE/CurrentRoom", order = 0)]
public class CurrentRoom : ScriptableObject
{
    public Vector2Int CurrentRoomPos;
    public int CurrentSpawn;
}
