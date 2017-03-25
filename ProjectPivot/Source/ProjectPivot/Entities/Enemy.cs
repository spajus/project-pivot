using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Components;

namespace ProjectPivot.Entities {
    public class Enemy : GameObject, Damageable {
        public GameObject Target;
        public Health Health;
        public Enemy(Vector2 position) : base(position) {
            AddComponent(new PawnBody());
            AddComponent(new PawnGraphics());
            AddComponent(new EnemyInput());
            AddComponent(new EnemyAI());
            Health = AddComponent<Health>(new Health(100));
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
    }
}
