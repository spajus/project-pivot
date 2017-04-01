using System;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Components {
    public class EnemyInput : PawnInput {
        public Vector2 Heading = Vector2.Zero;
        public bool InMotion = false;
        static Random random = new Random();

        public override void Initialize() {
            base.Initialize();
            this.maxSpeed = random.Next(300, 500) * 0.01f;
        }

        public override void Update(GameTime gameTime) {
            //PawnBody.Body.LinearVelocity = Heading * (float) gameTime.ElapsedGameTime.TotalSeconds;
            ApplyVelocity(gameTime, Heading.X, Heading.Y,
                changed: InMotion,
                normalize: false);
        }
    }
}
