using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using UnityEngine;

public class StatsHolder : BaseGameObject
{
    [field:SerializeField]
    public CounterStat HP { get; private set; }

    [field:SerializeField]
    public ObjectStats Stats { get; private set; }
}
