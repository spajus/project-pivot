using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPivot.Entities {
    public class BloodSplat : GameObject {
        float rotation;
        SpriteEffects sfx;
        public static Random random = new Random();
        public BloodSplat(Vector2 position) : base(position) {
            rotation = MathHelper.ToRadians(random.Next(360));
            switch (random.Next(4)) {
                case 0: { sfx = SpriteEffects.None; break; }
                case 1: { sfx = SpriteEffects.FlipVertically; break; }
                case 2: { sfx = SpriteEffects.FlipHorizontally; break; }
                case 3: { sfx = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically; break; }
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch,
                          "blood_splat",
                          Position,
                          layerDepth: 0.95f, // just above ground
                          rotation: rotation,
                          sfx: sfx);
        }
    }
}
