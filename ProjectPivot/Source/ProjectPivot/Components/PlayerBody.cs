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

namespace ProjectPivot.Components {
    internal class PlayerBody : Component {
        public Body Body { get; protected set; }

        public override void Initialize() {
            this.Body = BodyFactory.CreateRectangle(
                ProjectPivot.World,
                ConvertUnits.ToSimUnits(32),
                ConvertUnits.ToSimUnits(32),
                1.0f);
            Body.Mass = .01f;
            Body.BodyType = BodyType.Dynamic;
            Body.Restitution = .2f; //bounce
            Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
        }

        public override void Update(GameTime gameTime) {
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);

        }
    }
}
