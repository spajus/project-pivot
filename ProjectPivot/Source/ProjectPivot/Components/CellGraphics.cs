using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class CellGraphics : Component {
        private Health health;
        private static Vector2 cellOrigin = new Vector2(16, 16);
        private static Dictionary<int, string> textures;
        public float rotation;
        public SpriteEffects sfx;

        // 0 - no wall, 4 - full wall, 1-3 - in between
        public static void LoadContent(ContentManager content) {
            textures = new Dictionary<int, string>();
            textures.Add(0, "cell_00");
            textures.Add(1, "cell_25");
            textures.Add(2, "cell_50");
            textures.Add(3, "cell_75");
            textures.Add(4, "cell_100");
        }

        public override void Initialize() {
            Random random = Randomizer.Random;
            rotation = MathHelper.ToRadians(random.Next(4) * 90);
            switch (random.Next(4)) {
                case 0: { sfx = SpriteEffects.None; break; }
                case 1: { sfx = SpriteEffects.FlipVertically; break; }
                case 2: { sfx = SpriteEffects.FlipHorizontally; break; }
                case 3: { sfx = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally; break; }
            }
            health = GameObject.GetComponent<Health>();
        }

        public override void Update(GameTime gameTime) {
            //this.rotation += (float) gameTime.ElapsedGameTime.TotalSeconds;
            //base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch,
                          currentTexture(),
                          GameObject.Position,
                          layerDepth: 1f,
                          rotation: rotation,
                          sfx: this.sfx);

            if (Settings.DEBUG_CELL_HEALTH) {
                health.DrawHealth();
            }

            /*
            spriteBatch.Draw(
                currentTexture(),
                GameObject.Position,
                null, // Source rectangle
                Color.White,
                0f, // rotation
                cellOrigin, // Origin
                1f, // scale
                SpriteEffects.None,
                1f // layer depth
            );
*/
        }

        string currentTexture() {
			int textureNum = (int)MathHelper.Clamp(health.Value / 20f, 0, 4);
            return textures[textureNum];
        }
    }
}
