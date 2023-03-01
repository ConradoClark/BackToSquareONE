using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class TintFlash : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    [field: SerializeField]
    public float DurationInMs { get; private set; }

    [field: SerializeField]
    public Color Color { get; private set; }

    private Color _initialTint;

    protected override void OnAwake()
    {
        base.OnAwake();
        _initialTint = SpriteRenderer.material.GetColor("_Tint");
    }

    public void Flash()
    {
        DefaultMachinery.AddUniqueMachine($"TintFlash_{gameObject.GetInstanceID()}",
            UniqueMachine.UniqueMachineBehaviour.Replace,
            HandleFlash());
    }

    private IEnumerable<IEnumerable<Action>> HandleFlash()
    {
        SpriteRenderer.material.SetColor("_Tint", _initialTint);

        yield return SpriteRenderer.GetAccessor()
            .Material("_Tint")
            .AsColor()
            .ToColor(Color)
            .Over(DurationInMs * 0.00075f)
            .UsingTimer(GameTimer)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();

        yield return SpriteRenderer.GetAccessor()
            .Material("_Tint")
            .AsColor()
            .ToColor(_initialTint)
            .Over(DurationInMs * 0.00025f)
            .UsingTimer(GameTimer)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
    }
}
