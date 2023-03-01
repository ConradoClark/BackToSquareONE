using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics.CollisionDetection;

public class DestroyOnObstacleCollision : BaseGameObject
{
    public Bullet Bullet;
    public LichtPhysicsCollisionDetector CollisionDetector;

    protected override void OnEnable()
    {
        base.OnEnable();
        DefaultMachinery.AddBasicMachine(HandleCollision());
    }

    private IEnumerable<IEnumerable<Action>> HandleCollision()
    {
        while (ComponentEnabled)
        {
            if (CollisionDetector.Triggers.Any(t => t.TriggeredHit))
            {
                Bullet.EndEffect();
            }
            yield return TimeYields.WaitOneFrameX;
        }
    }
}