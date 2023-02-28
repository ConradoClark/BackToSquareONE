using System;
using System.Collections.Generic;
using Licht.Unity.Objects;
using UnityEngine;
public class Quadratica : Weapon
{
    public ScriptPrefab BulletPrefab;
    public Bullet.BulletParam[] Params;
    protected override Transform[] FindTargets()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<IEnumerable<Action>> Shoot(Transform target)
    {
        var direction = ((Vector2)(target.position - Holder.transform.position)).normalized;

        if (!BulletPrefab.TrySpawnBullet(Params, (Vector2)Holder.transform.position + direction * 0.5f, out var bullet))
            yield break;

        bullet.SetDirection(direction);
    }
}
