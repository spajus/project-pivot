using System;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Components {
    public class EnemyInput : PawnInput {
        public override void Update(GameTime gameTime) {
            ApplyVelocity(gameTime, 0, 0, false);
        }
    }
}
