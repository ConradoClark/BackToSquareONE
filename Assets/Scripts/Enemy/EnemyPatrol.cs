using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEditor;
using UnityEngine;

public class EnemyPatrol : BaseGameObject
{
    [field:SerializeField]
    public Vector2[] Points { get; private set; }

    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public PatrolType Type { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    private const float Distance = 0.1f;

    [Serializable]
    public enum PatrolType
    {
        BackToFirst,
        PingPong
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        DefaultMachinery.AddBasicMachine(HandlePatrol());
    }

    private IEnumerable<IEnumerable<Action>> HandlePatrol()
    {
        var route = Points;
        var initialIndex = 0;
        while (ComponentEnabled)
        {
            if (route.Length==0) yield return TimeYields.WaitOneFrameX;
            
            for (var i = initialIndex; i < route.Length; i++)
            {
                var point = route[i];
                while (Vector2.Distance(PhysicsObject.transform.position, point) > Distance)
                {
                    var distance = (point - (Vector2)PhysicsObject.transform.position).magnitude;
                    var direction = (point - (Vector2)PhysicsObject.transform.position).normalized;
                    PhysicsObject.ApplySpeed(direction * Mathf.Min(Speed, distance));
                    yield return TimeYields.WaitOneFrameX;
                }
            }

            switch (Type)
            {
                case PatrolType.PingPong:
                    route = route.Reverse().ToArray();
                    initialIndex = 1;
                    break;
                case PatrolType.BackToFirst:
                default:
                    initialIndex = 0;
                    break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        for (var index = 0; index < Points.Length; index++)
        {
            var p = Points[index];
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(p, Vector3.one * 0.5f);
            Handles.Label(p, index.ToString());
        }
    }
}
