using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class Player : BaseGameObject
{
    public LichtPlatformerMoveController MoveController { get; private set; }
    [field:SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }
    [field: SerializeField]
    public ScriptIdentifier GroundTrigger { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        MoveController = MoveController.FromScene();
    }

    public bool IsGrounded => PhysicsObject.GetPhysicsTrigger(GroundTrigger);
}
