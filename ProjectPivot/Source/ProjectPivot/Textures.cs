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
        private float initRotation;
        public TextureRegion(string atlasName, string name, int x, int y, int width, int height, float initRotation = 0f) {
            this.Name = name;
            this.AtlasName = atlasName;
            this.region = new Rectangle(x, y, width, height);
            this.initRotation = MathHelper.ToRadians(initRotation);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layerDepth = 0f, float rotation = 0f, SpriteEffects sfx = SpriteEffects.None) {
            spriteBatch.Draw(
                Textures.Atlas(AtlasName), 
                position, 
                region, 
                Color.White, 
                initRotation + rotation, 
                new Vector2(region.Width/2, region.Height/2), 
                Vector2.One,
                sfx,
                layerDepth);

        }
    }
    public static class Textures {
        private static Dictionary<string, Texture2D> atlases = new Dictionary<string, Texture2D>();
        private static Dictionary<string, TextureRegion> regions = new Dictionary<string, TextureRegion>();
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static void LoadContent(ContentManager content) {
            AddAtlas(content, "tilesheet_pa1", "Images/tilesheet_pa1");
            AddRegion(new TextureRegion("tilesheet_pa1", "player_down", 602, 226, 38, 38));
            AddRegion(new TextureRegion("tilesheet_pa1", "player_up", 640, 226, 38, 38));
            AddRegion(new TextureRegion("tilesheet_pa1", "player_left", 676, 226, 38, 38));
            AddRegion(new TextureRegion("tilesheet_pa1", "sniper_rifle", 565, 338, 38, 38, 45f));
            AddRegion(new TextureRegion("tilesheet_pa1", "small_bullet", 196, 308, 22, 22));
            AddTexture(content, "crosshair", "Images/crosshair");
        }

        public static Texture2D Atlas(string name) {
            return atlases[name];
        }

        public static void Draw(SpriteBatch spriteBatch, string regionName, Vector2 position, 
            float layerDepth = 0f, float rotation = 0f, SpriteEffects sfx = SpriteEffects.None) {
            regions[regionName].Draw(spriteBatch, position, layerDepth, rotation, sfx);
        }

        public static Texture2D Texture(string name) {
            return textures[name];
        }

        public static void AddRegion(TextureRegion region) {
            regions.Add(region.Name, region);
        }

        public static void AddTexture(ContentManager content, string name, string fileName) {
            textures.Add(name, content.Load<Texture2D>(fileName));
        }

        private static void AddAtlas(ContentManager content, string name, string fileName) {
            atlases.Add(name, content.Load<Texture2D>(fileName));
        }
    }
}
