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
    [field: SerializeField]
    public LichtPhysicsCollisionDetector BulletColliderDetector { get; private set; }

    [field: SerializeField]
    public LichtPhysicsCollisionDetector ContactCollisionDetector { get; private set; }

    [field: TagField]
    [field: SerializeField]
    public string TargetTag { get; private set; }

    [field: SerializeField]
    public float DamageCooldownInMs { get; private set; }

    public event Action OnAnyDamage;
    public event Action<EnemyContactDamage> OnDamageByContact;
    public event Action<Bullet> OnDamageByBullet;

    private LichtPhysics _physics;

    public bool IsInvulnerable { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (BulletColliderDetector != null) DefaultMachinery.AddBasicMachine(HandleDamage());
        if (ContactCollisionDetector != null) DefaultMachinery.AddBasicMachine(HandleContactDamage());
        _physics = this.GetLichtPhysics();
    }

    public void SetInvulnerability(bool invulnerability)
    {
        IsInvulnerable = invulnerability;
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

            if (triggered && !IsInvulnerable)
            {
                bullet.EndEffect();
                OnDamageByBullet?.Invoke(bullet);
                OnAnyDamage?.Invoke();

                if (DamageCooldownInMs > 0)
                {
                    yield return TimeYields.WaitMilliseconds(GameTimer, DamageCooldownInMs);
                }
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }

    private IEnumerable<IEnumerable<Action>> HandleContactDamage()
    {
        while (ComponentEnabled)
        {
            EnemyContactDamage enemy = null;
            var triggered = ContactCollisionDetector.Triggers.Any(t => t.TriggeredHit
                                                                       && _physics.TryGetPhysicsObjectByCollider(
                                                                           t.Collider, out var projectile)
                                                                       && projectile.TryGetCustomObject(out enemy)
                                                                       && enemy.Target == TargetTag);

            if (triggered && !IsInvulnerable)
            {
                OnDamageByContact?.Invoke(enemy);
                OnAnyDamage?.Invoke();

                if (DamageCooldownInMs > 0)
                {
                    yield return TimeYields.WaitMilliseconds(GameTimer, DamageCooldownInMs);
                }
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }
}
