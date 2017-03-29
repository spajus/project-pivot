using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using ProjectPivot.Components;
using ProjectPivot.Components.AI;

namespace ProjectPivot.Entities {
    public class Enemy : GameObject, Damageable {
        public GameObject Target;
        public Health Health;
        public Weapon Weapon;
        private EnemyInput input;
        private PawnBody body;
        public EnemyVision Vision;

        private EnemyState motionState;
        private EnemyState weaponState;

        public Enemy(Vector2 position) : base(position) {
            body = AddComponent<PawnBody>(new PawnBody());
            input = AddComponent<EnemyInput>(new EnemyInput());
            AddComponent(new PawnGraphics());
            Health = AddComponent<Health>(new Health(100));
            Vision = AddComponent<EnemyVision>(new EnemyVision());
            //AddComponent(new EnemyAI());
            ChangeMotionState(new MotionIdleState(this));
        }

        public void TakeWeapon(Weapon weapon) {
            weapon.Owner = this;
            Weapon = weapon;
            weapon.Initialize();
            input.Weapon = weapon;
            weaponState = new WeaponIdleState(this);
        }

        protected override void OnUpdate(GameTime gameTime) {
            ChangeMotionState(motionState.Update(gameTime));
            if (weaponState != null) {
                ChangeWeaponState(weaponState.Update(gameTime));
            }
        }

        public void ChangeMotionState(EnemyState newMotionState) {
            if (newMotionState != motionState) {
                if (motionState != null) {
                    motionState.Leave(newMotionState);
                }
                motionState = newMotionState;
                newMotionState.Enter(motionState);
            }
        }
        public void ChangeWeaponState(EnemyState newWeaponState) {
            if (newWeaponState != weaponState) {
                if (weaponState != null) {
                    weaponState.Leave(newWeaponState);
                }
                weaponState = newWeaponState;
                newWeaponState.Enter(weaponState);
            }
        }

        public bool TakeDamage(int damage, GameObject source) {
            if (source is Bullet) {
                GameObject shooter = ((Bullet)source).Shooter;
                if (shooter is Player) {
                    Target = shooter;
                }
            }
            Health.Decrease(damage);
            if (Health.Value <= 0f) {
                Weapon.Owner = null;
                ProjectPivot.World.RemoveBody(PhysicsBody());
                Destroy();
            }
            motionState.TakeDamage(damage, source);
            weaponState.TakeDamage(damage, source);
            GameObjects.Add(new BloodSplat(Position));
            AddComponent(new PawnBlood());
            return true;
        }

        public override Body PhysicsBody() {
            return body.Body;
        }
    }
}
