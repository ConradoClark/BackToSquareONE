using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class SkewWhenMoving : BaseGameObject
{
    public float Factor = 1;
    public float ScaleFactor = 1;
    public SpriteTransformer Controller;
    public LichtPhysicsObject PhysicsObject;

    private Vector3 _latest;
    private Vector3 _refSpeed;
    private Player _player;
    private bool _isMoving;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents,
            LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(
            LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStartMoving, OnStartMoving);

        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents,
            LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(
            LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStopMoving, OnStopMoving);

        DefaultMachinery.AddUniqueMachine("standingScale",
            UniqueMachine.UniqueMachineBehaviour.Wait, HandleStandingScale());
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this.StopObservingEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents,
            LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(
            LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStartMoving, OnStartMoving);

        this.StopObservingEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents,
            LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(
            LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStopMoving, OnStopMoving);
    }

    private void OnStopMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        _isMoving = false;
        DefaultMachinery.AddUniqueMachine("standingScale",
            UniqueMachine.UniqueMachineBehaviour.Wait, HandleStandingScale());
    }

    private void OnStartMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        _isMoving = true;
        DefaultMachinery.AddUniqueMachine("handleScale",
            UniqueMachine.UniqueMachineBehaviour.Wait, HandleScale());
    }

    private IEnumerable<IEnumerable<Action>> HandleStandingScale()
    {
        while (!_isMoving)
        {
            while (!_player.IsGrounded) yield return TimeYields.WaitOneFrameX;

            var scale = 0f;
            yield return new LerpBuilder(f => scale = f, () => scale)
                .SetTarget(1f)
                .Over(0.50f)
                .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
                .BreakIf(() => _isMoving)
                .OnEachStep(f => Controller.ApplyScale(new Vector3(f * ScaleFactor * 0.75f, f * ScaleFactor * 0.75f)))
                .Build();

            yield return new LerpBuilder(f => scale = f, () => scale)
                .SetTarget(0f)
                .Over(0.50f)
                .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
                .BreakIf(() => _isMoving)
                .OnEachStep(f => Controller.ApplyScale(new Vector3(f * ScaleFactor * 0.75f, f * ScaleFactor * 0.75f)))
                .Build();
        }
    }

    private IEnumerable<IEnumerable<Action>> HandleScale()
    {
        while (_isMoving)
        {
            while (!_player.IsGrounded) yield return TimeYields.WaitOneFrameX;

            var scale = 0f;
            yield return new LerpBuilder(f => scale = f, () => scale)
                .SetTarget(1f)
                .Over(0.25f)
                .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
                .OnEachStep(f => Controller.ApplyScale(new Vector3(0, f * ScaleFactor)))
                .Build();

            yield return new LerpBuilder(f => scale = f, () => scale)
                .SetTarget(0f)
                .Over(0.25f)
                .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
                .OnEachStep(f => Controller.ApplyScale(new Vector3(0, f * ScaleFactor)))
                .Build();
        }
    }

    private void Update()
    {
        var target = new Vector3(0, 0, Factor) * PhysicsObject.LatestSpeed.x * (_player.IsGrounded ? 1 : 0);
        var damp = Vector3.SmoothDamp(_latest, target, ref _refSpeed,
            1.0f, 1.0f, (float)GameTimer.UpdatedTimeInMilliseconds);
        Controller.ApplyRotation(Quaternion.Euler(-damp));
        _latest = damp;
    }

}
