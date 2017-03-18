using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;

namespace ProjectPivot.Components {
    public class CellBody : Component {
        public Body Body { get; protected set; }
        private Health health;

        public override void Initialize() {
            this.health = GameObject.GetComponent<Health>();
            if (health.IsHealthy) {
                this.Body = BodyFactory.CreateRectangle(
                    ProjectPivot.World,
                    ConvertUnits.ToSimUnits(32),
                    ConvertUnits.ToSimUnits(32),
                    1.0f);
                Body.Mass = 0.1f;
                Body.BodyType = BodyType.Static;
                Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
                Body.UserData = GameObject;
                Body.OnCollision += OnCollision;
           }
        }

        public override void Update(GameTime gameTime) {
            if (!health.IsHealthy && Body != null) {
                ProjectPivot.World.RemoveBody(Body);
                Body = null;
            }
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            if (fixtureB.Body.UserData is Bullet) {
                health.Decrease(25f);
            }
            return true;
        }
    }
}
