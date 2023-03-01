using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;

public class StraightLineTravel : BaseGameObject
{
    public Bullet Bullet;
    protected override void OnEnable()
    {
        base.OnEnable();
        DefaultMachinery.AddBasicMachine(HandleTravel());
    }

    private IEnumerable<IEnumerable<Action>> HandleTravel()
    {
        var dict = Bullet.BulletParams.ParamsDict;
        if (!dict.ContainsKey("Speed")) yield break;
        while (ComponentEnabled)
        {
            Bullet.PhysicsObject.ApplySpeed(Bullet.Direction * dict["Speed"]);
            yield return TimeYields.WaitOneFrameX;
        }
    }
}
