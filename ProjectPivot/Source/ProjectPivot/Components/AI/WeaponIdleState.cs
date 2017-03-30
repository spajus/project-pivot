using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Components.AI {
    public class WeaponIdleState : EnemyState {
        public WeaponIdleState(Enemy enemy) : base(enemy) {
        }

        public override EnemyState Update(GameTime gameTime) {
            Player target = enemy.Vision.FindVisible<Player>();
            if (target != null) {
                return new WeaponAttackingState(enemy, target);
            }
            return this;
        }
    }
}
