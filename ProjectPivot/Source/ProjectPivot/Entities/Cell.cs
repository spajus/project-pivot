using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public class Cell {
        private static Dictionary<int, Texture2D> textures;

        public float Health { get; protected set; }
        public Vector2 Position { get; protected set; }
        public int MapX { get; protected set; }
        public int MapY { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Rectangle Area { get; protected set; }

        // 0 - no wall, 4 - full wall, 1-3 - in between
        public static void LoadContent(ContentManager content) {
            textures = new Dictionary<int, Texture2D>();
            textures.Add(0, content.Load<Texture2D>(@"Images/cell_00"));
            textures.Add(1, content.Load<Texture2D>(@"Images/cell_25"));
            textures.Add(2, content.Load<Texture2D>(@"Images/cell_50"));
            textures.Add(3, content.Load<Texture2D>(@"Images/cell_75"));
            textures.Add(4, content.Load<Texture2D>(@"Images/cell_100"));
        }

        public Cell(int mapX, int mapY, int width, int height, int health) {
            this.Health = health;
            this.Width = width;
            this.Height = height;
            this.MapX = mapX;
            this.MapY = mapY;
            this.Position = new Vector2(MapX * Width, MapY * Height);
            this.Area = new Rectangle((int) Position.X, (int) Position.Y, width, height);
        }


        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(currentTexture(), Position, Color.White);
        }

        Texture2D currentTexture() {
			int textureNum = (int)MathHelper.Clamp(Health / 20f, 0, 4);
            return textures[textureNum];
        }
    }
}
