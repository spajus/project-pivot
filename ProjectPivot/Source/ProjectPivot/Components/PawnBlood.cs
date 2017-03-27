using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPivot.Components {
    public class PawnBlood : Component {
        float rotation;
        SpriteEffects sfx;
        public static Random random = new Random();

        public void Initialize() {
            rotation = MathHelper.ToRadians(random.Next(360));
            switch (random.Next(4)) {
                case 0: { sfx = SpriteEffects.None; break; }
                case 1: { sfx = SpriteEffects.FlipVertically; break; }
                case 2: { sfx = SpriteEffects.FlipHorizontally; break; }
                case 3: { sfx = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically; break; }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch,
                          "blood_splat",
                          GameObject.Position,
                          layerDepth: 0.18f, // just above pawn
                          rotation: rotation,
                          sfx: sfx);
        }
    }
}
