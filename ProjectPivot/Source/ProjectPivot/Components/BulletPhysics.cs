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
            Body.LinearDamping = 1f;
            Body.IgnoreCollisionWith(Shooter.PhysicsBody());
            Body.CollisionCategories = Category.Cat11;
            Body.IgnoreCCDWith = Category.Cat11;
            Body.OnCollision += OnCollision;
            Body.UserData = GameObject;
            Vector2 shotForce = (Target - GameObject.Position);
            shotForce.Normalize();
            Body.ApplyLinearImpulse(ConvertUnits.ToSimUnits(shotForce) * 20f);
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            GameObject.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            if (fixtureB.Body.UserData is Damageable) {
                int damage = (int)Math.Round(Body.LinearVelocity.LengthSquared() / 100);
                bool hit = ((Damageable)fixtureB.Body.UserData).TakeDamage(damage, GameObject);
                if (hit) {
                    TempText.Write(GameObject.Position,
                                   damage.ToString(),
                                   Color.Pink,
                                   1000f,
                                   2f);
                    GameObject.Destroy();
                }
                return hit;
            }
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
