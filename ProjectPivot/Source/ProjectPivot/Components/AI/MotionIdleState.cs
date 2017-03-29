using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components.AI {
    public class MotionIdleState : EnemyState {
        private static Random random = new Random();
        private float timeLeftToIdleMs;
        public MotionIdleState(Enemy enemy) : base(enemy) { 
            timeLeftToIdleMs = random.Next(1000, 3000);
        }

        public override EnemyState Update(GameTime gameTime) {
            timeLeftToIdleMs -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeLeftToIdleMs < 0f) {
                return new MotionRoamingState(enemy);
            }
            return this;
        }
    }
}
