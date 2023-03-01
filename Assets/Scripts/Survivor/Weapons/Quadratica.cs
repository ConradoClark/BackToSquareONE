using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
public class Quadratica : Weapon
{
    public ScriptPrefab BulletPrefab;
    public Bullet.BulletParam[] Params;

    private RoomManager _roomManager;
    protected override void OnAwake()
    {
        base.OnAwake();
        _roomManager = _roomManager.FromScene();
    }

    protected override Transform[] FindTargets()
    {
        var first = _roomManager.GetCurrentRoom()
            .GetComponentsInChildren<Enemy>()
            .FirstOrDefault();

        return first != null ? new[] { first.transform } : null;
    }

    protected override IEnumerable<IEnumerable<Action>> Shoot(Transform target)
    {
        var direction = ((Vector2)(target.position - Holder.transform.position ) - Holder.SpawnOffset).normalized;

        if (!BulletPrefab.TrySpawnBullet(Params, (Vector2)Holder.transform.position
                                                 + Holder.SpawnOffset
                                                 + direction * 0.5f, Holder.Target, out var bullet))
            yield break;

        bullet.SetDirection(direction);
    }
}
