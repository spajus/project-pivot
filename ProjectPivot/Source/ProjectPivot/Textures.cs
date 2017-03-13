using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot {
    public class TextureRegion {
        public string Name { get; protected set; }
        public string AtlasName { get; protected set; }
        private Rectangle region;
        public TextureRegion(string atlasName, string name, int x, int y, int width, int height) {
            this.Name = name;
            this.AtlasName = atlasName;
            this.region = new Rectangle(x, y, width, height);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position) {
            spriteBatch.Draw(Textures.Atlas(AtlasName), position, region, Color.White);
        }
    }
    public static class Textures {
        private static Dictionary<string, Texture2D> atlases = new Dictionary<string, Texture2D>();
        private static Dictionary<string, TextureRegion> regions = new Dictionary<string, TextureRegion>();

        public static void LoadContent(ContentManager content) {
            AddAtlas(content, "tilesheet_pa1", "Images/tilesheet_pa1");
            AddRegion(new TextureRegion("tilesheet_pa1", "player_down", 602, 226, 38, 38));
        }

        public static Texture2D Atlas(string name) {
            return atlases[name];
        }

        public static void Draw(SpriteBatch spriteBatch, string regionName, Vector2 position) {
            regions[regionName].Draw(spriteBatch, position);
        }

        public static void AddRegion(TextureRegion region) {
            regions.Add(region.Name, region);
        }

        private static void AddAtlas(ContentManager content, string name, string fileName) {
            atlases.Add(name, content.Load<Texture2D>(fileName));
        }
    }
}
