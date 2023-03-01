using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;

public class TintFlashOnDamage : BaseGameObject
{
    public TintFlash TintFlash;
    public Damageable Damageable;

    protected override void OnEnable()
    {
        base.OnEnable();
        Damageable.OnDamageByBullet += OnDamage;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Damageable.OnDamageByBullet -= OnDamage;
    }

    private void OnDamage(Bullet obj)
    {
        TintFlash.Flash();
    }
}
