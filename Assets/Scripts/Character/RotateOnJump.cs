using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotateOnJump : BaseGameObject
{
    [field: SerializeField] public SpriteTransformer Transformer { get; private set; }
    private Player _player;
    private IEnumerable<IEnumerable<Action>>[] _patterns;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
        _patterns = new[]
        {
            RotationPattern1(),
            RotationPattern2(),
            RotationPattern3(),
            RotationPattern4(),
        };
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this.ObserveEvent<LichtPlatformerJumpController.LichtPlatformerJumpEvents,
            LichtPlatformerJumpController.LichtPlatformerJumpEventArgs>(
            LichtPlatformerJumpController.LichtPlatformerJumpEvents.OnJumpStart, OnJumpStart);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this.StopObservingEvent<LichtPlatformerJumpController.LichtPlatformerJumpEvents,
            LichtPlatformerJumpController.LichtPlatformerJumpEventArgs>(
            LichtPlatformerJumpController.LichtPlatformerJumpEvents.OnJumpStart, OnJumpStart);
    }

    private void OnJumpStart(LichtPlatformerJumpController.LichtPlatformerJumpEventArgs obj)
    {
        DefaultMachinery.AddUniqueMachine("jumpRotate", UniqueMachine.UniqueMachineBehaviour.Replace,
            HandleRotation());
    }

    private IEnumerable<IEnumerable<Action>> HandleRotation()
    {
        yield return TimeYields.WaitMilliseconds(GameTimer, 200);

        var chosen = _patterns[Random.Range(0, _patterns.Length)];
        yield return chosen.AsCoroutine();
    }

    private IEnumerable<IEnumerable<Action>> RotationPattern1()
    {
        yield return new LerpBuilder()
            .SetTarget(360)
            .Over(0.45f)
            .OnEachStep(f => Transformer.ApplyRotation(Quaternion.Euler(0, 0, f)))
            .BreakIf(()=> _player.IsGrounded)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> RotationPattern2()
    {
        yield return new LerpBuilder()
            .SetTarget(180)
            .Over(0.35f)
            .OnEachStep(f => Transformer.ApplyRotation(Quaternion.Euler(0, 0, f)))
            .BreakIf(() => _player.IsGrounded)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> RotationPattern3()
    {
        yield return new LerpBuilder()
            .SetTarget(180)
            .Over(0.35f)
            .OnEachStep(f => Transformer.ApplyRotation(Quaternion.Euler(0, 0, -f)))
            .BreakIf(() => _player.IsGrounded)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> RotationPattern4()
    {
        yield return new LerpBuilder()
            .SetTarget(360)
            .Over(0.45f)
            .OnEachStep(f => Transformer.ApplyRotation(Quaternion.Euler(0, 0, -f)))
            .BreakIf(() => _player.IsGrounded)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();
    }
}
