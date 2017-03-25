using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using ProjectPivot.Components;

namespace ProjectPivot.Entities {
    public class Enemy : GameObject, Damageable {
        public GameObject Target;
        public Health Health;
        public Weapon Weapon;
        private EnemyInput input;
        private PawnBody body;
        public Enemy(Vector2 position) : base(position) {
            body = AddComponent<PawnBody>(new PawnBody());
            AddComponent(new PawnGraphics());
            input = AddComponent<EnemyInput>(new EnemyInput());
            AddComponent(new EnemyAI());
            Health = AddComponent<Health>(new Health(100));
        }

        public void TakeWeapon(Weapon weapon) {
            weapon.Owner = this;
            Weapon = weapon;
            weapon.Initialize();
            input.Weapon = weapon;
        }

        public bool TakeDamage(int damage, GameObject source) {
            if (source is Bullet) {
                Target = ((Bullet)source).Shooter;
            }
            Health.Decrease(damage);
            if (Health.Value <= 0f) {
                Destroy();
            }
            return true;
        }

        public override Body PhysicsBody() {
            return body.Body;
        }
    }
}
