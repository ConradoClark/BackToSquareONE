using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Licht.Unity.Objects;
using UnityEngine;

public class WeaponHolder : BaseGameObject
{
    [field: SerializeField]
    public GameObject Owner { get; private set; }

    [field:SerializeField]
    public Weapon[] Loadout { get; private set; }

    [field:TagField]
    [field: SerializeField]
    public string Target { get; private set; }

}
