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
        float lifetime = 0f;
        public float MaxLifeTime = 20000f;

        public BulletPhysics(GameObject shooter, Vector2 target) {
            Target = target;
            Shooter = shooter;
        }

        public override void Initialize() {
            this.Body = BodyFactory.CreateCircle(
                ProjectPivot.World,
                ConvertUnits.ToSimUnits(4),
                1.0f);
            Body.Mass = 0.01f;
            Body.IsBullet = true;
            Body.FixedRotation = true;
            Body.Friction = 0.2f;
            Body.Restitution = 0.02f;
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
            Body.LinearDamping = 3f;
            Body.IgnoreCollisionWith(Shooter.PhysicsBody());
            Body.OnCollision += OnCollision;
            Body.UserData = GameObject;
            Vector2 shotForce = (Target - GameObject.Position);
            shotForce.Normalize();
            Body.ApplyForce(ConvertUnits.ToSimUnits(shotForce) * 2000f);
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            //GameObject.Destroy();
            return true;
        }

        public override void Update(GameTime gameTime) {
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            lifetime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (lifetime > MaxLifeTime) {
                GameObject.Destroy();
            }
        }
    }
}
