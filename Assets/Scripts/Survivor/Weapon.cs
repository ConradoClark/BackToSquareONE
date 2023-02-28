using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public abstract class Weapon : BaseGameObject
{
    protected WeaponHolder Holder { get; private set; }
    protected abstract Transform[] FindTargets();
    protected abstract IEnumerable<IEnumerable<Action>> Shoot(Transform target);

    protected Transform[] CurrentTargets { get; private set; }

    [field:SerializeField]
    public Sprite IconSprite { get; private set; }

    [field: SerializeField]
    protected float CooldownInMs { get; set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        Holder = GetComponentInParent<WeaponHolder>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DefaultMachinery.AddBasicMachine(HandleWeapon());
    }

    private IEnumerable<IEnumerable<Action>> HandleWeapon()
    {
        while (ComponentEnabled)
        {
            CurrentTargets = FindTargets();

            if (CurrentTargets == null || CurrentTargets.Length == 0)
            {
                yield return TimeYields.WaitMilliseconds(GameTimer, 100);
            }
            else
            {
                foreach (var target in CurrentTargets)
                {
                    DefaultMachinery.AddBasicMachine(Shoot(target));
                }

                yield return TimeYields.WaitMilliseconds(GameTimer, Mathf.Max(10, CooldownInMs));
            }
        }
    }
}
