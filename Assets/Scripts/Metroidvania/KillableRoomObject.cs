using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class KillableRoomObject : BaseGameObject
{
    [field:SerializeField]
    public Killable Killable { get; private set; }

    [field: SerializeField]
    public RoomObject RoomObject { get; private set; }

    [field: SerializeField]
    public ScriptPrefab KillEffect { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        Killable.OnDeath += Killable_OnDeath;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Killable.OnDeath -= Killable_OnDeath;
    }

    private void Killable_OnDeath()
    {
        RoomObject.Deactivate();
        if (KillEffect != null)
        {
            KillEffect.TrySpawnEffect(RoomObject.transform.position, out _);
        }
    }
}
