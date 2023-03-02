using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class HPDamageOnWeaponHit : BaseGameObject
{
    [field: SerializeField]
    public Damageable Damageable { get; private set; }
    
    [field: SerializeField]
    public CounterStat HP { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        Damageable.OnDamageByBullet += OnDamage;
        Damageable.OnDamageByContact += OnDamageByContact;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Damageable.OnDamageByBullet -= OnDamage;
        Damageable.OnDamageByContact -= OnDamageByContact;
    }

    private void OnDamage(Bullet obj)
    {
        if (!obj.BulletParams.ParamsDict.ContainsKey("Damage")) return;
        var damage = Mathf.RoundToInt(obj.BulletParams.ParamsDict["Damage"]);

        HP.Value-=damage;
    }

    private void OnDamageByContact(EnemyContactDamage obj)
    {
        HP.Value -= obj.Damage;
    }

}
