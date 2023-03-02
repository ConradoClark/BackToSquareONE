using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class KnockbackOnDamage : BaseGameObject
{
    [field:SerializeField]
    public Damageable Damageable { get; private set; }

    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public LichtPlatformerMoveController MoveController { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        Damageable.OnDamageByBullet += Damageable_OnDamageByBullet;
        Damageable.OnDamageByContact += Damageable_OnDamageByContact;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Damageable.OnDamageByBullet -= Damageable_OnDamageByBullet;
        Damageable.OnDamageByContact -= Damageable_OnDamageByContact;
    }

    private void Damageable_OnDamageByBullet(Bullet obj)
    {
       var strength = obj.BulletParams.ParamsDict.ContainsKey("KnockBack") ? obj.BulletParams.ParamsDict["KnockBack"] : 1;
        DefaultMachinery.AddBasicMachine(KnockBack(obj.Direction * strength));
    }
    private void Damageable_OnDamageByContact(EnemyContactDamage obj)
    {
        var xSpeed = MoveController == null ? PhysicsObject.LatestSpeed.x : MoveController.LatestDirection*5f;
        DefaultMachinery.AddBasicMachine(KnockBack(
            new Vector2(-xSpeed * obj.KnockBackMultiplier, 0.5f)));
    }

    private IEnumerable<IEnumerable<Action>> KnockBack(Vector2 speed)
    {
        yield return PhysicsObject.GetSpeedAccessor(speed)
            .ToSpeed(Vector2.zero)
            .Over(0.5f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .UsingTimer(GameTimer)
            .Build();
    }
}
