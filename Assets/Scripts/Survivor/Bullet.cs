using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;

public class Bullet : EffectPoolable
{
    [field:SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public BulletParamsContainer BulletParams { get; private set; }

    [field: TagField]
    public string Target { get; set; }

    public Vector2 Direction { get; private set; }


    [Serializable]
    public struct BulletParam
    {
        public string Key;
        public float Value;
    }

    [Serializable]
    public class BulletParamsContainer
    {
        public BulletParam[] Params;
        public IDictionary<string, float> ParamsDict => Params.ToDictionary(k => k.Key, v => v.Value);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        PhysicsObject.AddCustomObject(this);
    }

    public override void OnActivation()
    {
        
    }

    public void SetParams(BulletParam[] bulletParams)
    {
        BulletParams ??= new BulletParamsContainer();
        BulletParams.Params = bulletParams.ToArray();
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction.normalized;
    }
}
