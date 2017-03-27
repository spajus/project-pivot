using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPivot.Components {
    public class PawnBlood : Component {
        float rotation;
        SpriteEffects sfx;
        string textureName;
        public float LayerDepth = 0.18f; // just above pawn
        public static Random random = new Random();

        public PawnBlood() {
            rotation = MathHelper.ToRadians(random.Next(360));
            int texNum = random.Next(1, 3);
            this.textureName = $"blood_splat{texNum}";

            switch (random.Next(4)) {
                case 0: { sfx = SpriteEffects.None; break; }
                case 1: { sfx = SpriteEffects.FlipVertically; break; }
                case 2: { sfx = SpriteEffects.FlipHorizontally; break; }
                case 3: { sfx = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically; break; }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch,
                          textureName,
                          GameObject.Position,
                          layerDepth: LayerDepth,
                          rotation: rotation,
                          sfx: sfx);
        }
    }
}
