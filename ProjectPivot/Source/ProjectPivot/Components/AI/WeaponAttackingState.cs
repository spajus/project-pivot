using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using ProjectPivot.Utils;
using FarseerPhysics.Collision;

namespace ProjectPivot.Components.AI {
    public class WeaponAttackingState : EnemyState {
        Player target;
        EnemyInput input;
        const float shootingDistanceSquared = 15 * 32 * 15 * 32;
        float shotCooldownMs = 0f;
        public WeaponAttackingState(Enemy enemy, Player target) : base(enemy) {
            input = enemy.GetComponent<EnemyInput>();
            this.target = target;
        }

        public override EnemyState Update(GameTime gameTime) {
            if (Vector2.DistanceSquared(enemy.Position, target.Position) < shootingDistanceSquared) {
                shotCooldownMs -= gameTime.ElapsedGameTime.Milliseconds;
                Vector2 toTarget = target.Position - enemy.Position;
                toTarget.Normalize();
                input.Rotation = MathHelper.ToRadians(90) - (float)Math.Atan2(toTarget.X, toTarget.Y);
                if (shotCooldownMs <= 0f) {
                    RaycastHit hit = PhysicsTools.RaycastFirst(enemy.Position, target.Position);
                    Random random = Randomizer.Random;
                    if (hit != null && hit.Fixture.Body.UserData == target) {
                        // hand shakes
                        Vector2 offset = new Vector2(
                            random.Next(-32, 32),
                            random.Next(-32, 32));
                        enemy.Weapon.Fire(target.Position + offset);
                    }
                    shotCooldownMs = random.Next(500, 2000);
                }
                return this;
            }
            return new WeaponIdleState(enemy);
        }

    }
}
