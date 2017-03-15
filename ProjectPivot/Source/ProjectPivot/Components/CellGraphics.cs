using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class CellGraphics : Component {
        private Health health;
        private static Dictionary<int, Texture2D> textures;
        // 0 - no wall, 4 - full wall, 1-3 - in between
        public static void LoadContent(ContentManager content) {
            textures = new Dictionary<int, Texture2D>();
            textures.Add(0, content.Load<Texture2D>(@"Images/cell_00"));
            textures.Add(1, content.Load<Texture2D>(@"Images/cell_25"));
            textures.Add(2, content.Load<Texture2D>(@"Images/cell_50"));
            textures.Add(3, content.Load<Texture2D>(@"Images/cell_75"));
            textures.Add(4, content.Load<Texture2D>(@"Images/cell_100"));
        }

        public override void Initialize() {
            health = GameObject.GetComponent<Health>();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                currentTexture(),
                GameObject.Position,
                null, // Source rectangle
                Color.White,
                0f, // rotation
                Vector2.Zero, // Origin
                1f, // scale
                SpriteEffects.None,
                1f // layer depth
            );
        }

        Texture2D currentTexture() {
			int textureNum = (int)MathHelper.Clamp(health.Value / 20f, 0, 4);
            return textures[textureNum];
        }
    }
}
