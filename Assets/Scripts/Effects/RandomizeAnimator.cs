using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizeAnimator : BaseGameObject
{
    [field:SerializeField]
    public RuntimeAnimatorController[] Controllers { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        Animator.runtimeAnimatorController = Controllers[Random.Range(0, Controllers.Length)];
    }
}
 