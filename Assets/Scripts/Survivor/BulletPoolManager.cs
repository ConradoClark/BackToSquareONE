using System.Collections;
using System.Linq;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class BulletPoolManager : CustomPrefabManager<BulletPool, Bullet>
{
}


public static class BulletPoolExtensions
{
    public static bool TrySpawnBullet(this ScriptPrefab bulletPrefab, Bullet.BulletParam[] bulletParams,
        Vector2 position, string bulletTarget,
        out Bullet bullet)
    {
        if (!SceneObject<BulletPoolManager>.Instance().GetEffect(bulletPrefab).TryGetFromPool(out bullet))
        {
            return false;
        }

        bullet.Target = bulletTarget;
        bullet.SetParams(bulletParams);
        bullet.transform.position = position;
        return true;
    }
}