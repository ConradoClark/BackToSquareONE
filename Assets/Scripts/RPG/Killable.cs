using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class Killable : BaseGameObject
{
    [field:SerializeField]
    public CounterStat HP { get; private set; }

    public event Action OnDeath;

    protected override void OnEnable()
    {
        base.OnEnable();
        HP.OnChange += OnHPChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        HP.OnChange -= OnHPChange;
    }

    private void OnHPChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        if (obj.OldValue == obj.NewValue || obj.NewValue > 0) return;
        OnDeath?.Invoke();
    }
}
