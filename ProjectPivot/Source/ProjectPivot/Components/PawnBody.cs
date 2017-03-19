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
    public class PawnBody : Component {
        public Body Body { get; protected set; }
        public override void Initialize() {
            this.Body = BodyFactory.CreateCircle(
                ProjectPivot.World,
                ConvertUnits.ToSimUnits(15),
                //ConvertUnits.ToSimUnits(32),
                1.0f);
            Body.Mass = 1f;
            Body.BodyType = BodyType.Dynamic;
			Body.Friction = 0.001f;
            Body.FixedRotation = true;
            Body.Restitution = .0f; //bounce
            Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
            Body.LinearDamping = 0.001f;
            Body.UserData = GameObject;
        }

        public override void Update(GameTime gameTime) {
            Vector2 lastPos = GameObject.Position;
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            if (lastPos != GameObject.Position) {
                GameObjects.UpdatePosition(GameObject);
            }
        }
    }
}
