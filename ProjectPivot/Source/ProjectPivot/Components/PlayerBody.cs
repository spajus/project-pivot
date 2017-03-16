using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Utils;
using FarseerPhysics.Dynamics.Contacts;

namespace ProjectPivot.Components {
    internal class PlayerBody : Component {
        public Body Body { get; protected set; }
        public bool IsColliding { get; protected set; }

        public override void Initialize() {
            this.Body = BodyFactory.CreateCircle(
                ProjectPivot.World,
                ConvertUnits.ToSimUnits(16),
                //ConvertUnits.ToSimUnits(32),
                1.0f);
            Body.Mass = 100f;
            Body.BodyType = BodyType.Dynamic;
			Body.Friction = 0.01f;
            Body.FixedRotation = true;
            Body.Restitution = .0f; //bounce
            Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
            Body.LinearDamping = 5f;
        }

        public override void Update(GameTime gameTime) {
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);
        }
    }
}
