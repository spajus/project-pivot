using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using ProjectPivot.Utils;

namespace ProjectPivot.Components {
    public class BulletPhysics : Component {
        public Body Body { get; protected set; }
        public Vector2 Target;
        public GameObject Shooter;

        public BulletPhysics(GameObject shooter, Vector2 target) {
            Target = target;
            Shooter = shooter;
        }

        public override void Initialize() {
            this.Body = BodyFactory.CreateCircle(
                ProjectPivot.World,
                ConvertUnits.ToSimUnits(2),
                1.0f);
            Body.Mass = 0.01f;
            Body.IsBullet = true;
            Body.FixedRotation = true;
            Body.Friction = 0.001f;
            Body.Restitution = 0.2f;
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
            Body.LinearDamping = 0.0001f;
            Body.IgnoreCollisionWith(Shooter.PhysicsBody());
            Body.OnCollision += OnCollision;
            Body.UserData = GameObject;
            Vector2 shotForce = (Target - GameObject.Position);
            shotForce.Normalize();
            Body.ApplyForce(ConvertUnits.ToSimUnits(shotForce) * 300f);
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            GameObjects.Destroy(GameObject);
            return true;
        }

        public override void Update(GameTime gameTime) {
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);
        }
    }
}
