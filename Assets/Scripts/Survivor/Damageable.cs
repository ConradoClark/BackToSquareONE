using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class Damageable : BaseGameObject
{
    [field:SerializeField]
    public LichtPhysicsCollisionDetector BulletColliderDetector { get; private set; }
    
    [field: TagField]
    [field: SerializeField]
    public string TargetTag { get; private set; }

    [field: SerializeField]
    public float DamageCooldownInMs { get; private set; }

    public event Action<Bullet> OnDamageByBullet;

    private LichtPhysics _physics;

    protected override void OnEnable()
    {
        base.OnEnable();
        DefaultMachinery.AddBasicMachine(HandleDamage());
        _physics = this.GetLichtPhysics();
    }

    private IEnumerable<IEnumerable<Action>> HandleDamage()
    {
        while (ComponentEnabled)
        {
            Bullet bullet = null;
            var triggered = BulletColliderDetector.Triggers.Any(t => t.TriggeredHit
                                                                     && _physics.TryGetPhysicsObjectByCollider(
                                                                         t.Collider, out var projectile)
                                                                     && projectile.TryGetCustomObject(out bullet)
                                                                     && bullet.Target == TargetTag);

            if (triggered)
            {
                bullet.EndEffect();
                OnDamageByBullet?.Invoke(bullet);

                if (DamageCooldownInMs > 0)
                {
                    yield return TimeYields.WaitMilliseconds(GameTimer, DamageCooldownInMs);
                }
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }
}
