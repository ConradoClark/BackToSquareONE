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

public class ScaleOnJump : BaseGameObject
{
    [field: SerializeField]
    public SpriteTransformer Transformer { get; private set; }

    [field: SerializeField]
    public float XFactor { get; private set; }

    [field: SerializeField]
    public float YFactor { get; private set; }

    private Player _player;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this.ObserveEvent<LichtPlatformerJumpController.LichtPlatformerJumpEvents,
            LichtPlatformerJumpController.LichtPlatformerJumpEventArgs>(
            LichtPlatformerJumpController.LichtPlatformerJumpEvents.OnJumpStart, OnJumpStart);

        DefaultMachinery.AddBasicMachine(EffectOnGrounded());
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
        DefaultMachinery.AddUniqueMachine("jumpStretch",
            UniqueMachine.UniqueMachineBehaviour.Replace, Squash()
                .Then(UnSquash()));
    }

    private IEnumerable<IEnumerable<Action>> Squash()
    {
        yield return new LerpBuilder()
            .SetTarget(1f)
            .Over(0.15f)
            .OnEachStep(f => Transformer.ApplyScale(new Vector3(f * XFactor, -f * YFactor)))
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> UnSquash(float intensity = 1f)
    {
        yield return new LerpBuilder(1f)
            .SetTarget(0f)
            .Over(0.25f)
            .OnEachStep(f => Transformer.ApplyScale(new Vector3(f * XFactor * intensity, -f * YFactor)))
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> EffectOnGrounded()
    {
        while (ComponentEnabled)
        {
            while (!_player.IsGrounded) yield return TimeYields.WaitOneFrameX;

            yield return UnSquash(Mathf.Max(0, -_player.PhysicsObject.LatestNonZeroSpeed.y*0.075f)).AsCoroutine();

            while (_player.IsGrounded) yield return TimeYields.WaitOneFrameX;
        }
    }
}
