using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Components.AI {
    public class WeaponAttackingState : EnemyState {
        Player target;
        EnemyInput input;
        const float shootingDistanceSquared = 15 * 32 * 15 * 32;
        public WeaponAttackingState(Enemy enemy, Player target) : base(enemy) {
            input = enemy.GetComponent<EnemyInput>();
            this.target = target;
        }

        public override EnemyState Update(GameTime gameTime) {
            if (Vector2.DistanceSquared(enemy.Position, target.Position) < shootingDistanceSquared) {
                Vector2 toTarget = target.Position - enemy.Position;
                toTarget.Normalize();
                input.Rotation = MathHelper.ToRadians(90) - (float)Math.Atan2(toTarget.X, toTarget.Y);
                enemy.Weapon.Fire(target.Position);
                return this;
            }
            return new WeaponIdleState(enemy);
        }
    }
}
