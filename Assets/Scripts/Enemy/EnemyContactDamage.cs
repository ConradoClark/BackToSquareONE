using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class EnemyContactDamage : BaseGameObject
{
    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public float KnockBackMultiplier { get; private set; }

    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: TagField]
    [field: SerializeField]
    public string Target { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        PhysicsObject.AddCustomObject(this);
    }
}
