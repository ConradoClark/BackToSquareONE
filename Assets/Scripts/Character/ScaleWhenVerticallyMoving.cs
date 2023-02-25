using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

internal class ScaleWhenVerticallyMoving : BaseGameObject
    {
        public float Factor = 1;
        public SpriteTransformer Controller;
        public LichtPhysicsObject PhysicsObject;

        private float _latest;
        private float _refSpeed;

        private void Update()
        {
            var target = Factor * PhysicsObject.LatestCalculatedSpeed.y;
            var damp = Mathf.SmoothDamp(_latest, target, ref _refSpeed,
                1.0f, 0.05f, (float)GameTimer.UpdatedTimeInMilliseconds);

            Controller.ApplyScale(new Vector2(0, damp));
            _latest = damp;
        }

    }
