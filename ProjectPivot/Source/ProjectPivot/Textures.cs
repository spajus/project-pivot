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
        public int textureScale = 1;
        public TextureRegion(string atlasName, string name, int x, int y, int width, int height, float initRotation = 0f, int textureScale = 1) {
            this.Name = name;
            this.AtlasName = atlasName;
            this.region = new Rectangle(x, y, width, height);
            this.initRotation = MathHelper.ToRadians(initRotation);
            this.textureScale = textureScale;
        }
        public void Draw(
            SpriteBatch spriteBatch, 
            Vector2 position, 
            float layerDepth = 0f, 
            float rotation = 0f,
            SpriteEffects sfx = SpriteEffects.None) {
            spriteBatch.Draw(
                Textures.Atlas(AtlasName),
                position,
                region,
                Color.White,
                initRotation + rotation,
                new Vector2(region.Width / 2, region.Height / 2),
                Vector2.One / textureScale,
                sfx,
                layerDepth);

        }
    }
    public static class Textures {
        private static Dictionary<string, Texture2D> atlases = new Dictionary<string, Texture2D>();
        private static Dictionary<string, TextureRegion> regions = new Dictionary<string, TextureRegion>();
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public static Effect Shader1;

        public static void LoadContent(ContentManager content) {
            AddAtlas(content, "tilesheet_pp", "Images/tilesheet_pp");
            AddRegion(new TextureRegion("tilesheet_pp",
                                        "cell_00",
                                        0, 0, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp",
                                        "cell_25",
                                        64, 0, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp",
                                        "cell_50",
                                        128, 0, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp",
                                        "cell_75",
                                        192, 0, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp",
                                        "cell_100",
                                        256, 0, 64, 64, textureScale: 2));

            AddRegion(new TextureRegion("tilesheet_pp",
                                        "debris1",
                                        256, 64, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp",
                                        "debris2",
                                        320, 64, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp",
                                        "debris3",
                                        384, 64, 64, 64, textureScale: 2));

            AddRegion(new TextureRegion("tilesheet_pp", "player_down", 0, 128, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp", "player_up", 64, 128, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp", "player_left", 128, 128, 64, 64, textureScale: 2));

            AddRegion(new TextureRegion("tilesheet_pp", "sniper_rifle", 0, 192, 64, 64, 45f, textureScale: 2));


            AddRegion(new TextureRegion("tilesheet_pp", "small_bullet", 25, 89, 14, 14, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp", "blood_splat1", 64, 64, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp", "blood_splat2", 128, 64, 64, 64, textureScale: 2));
            AddRegion(new TextureRegion("tilesheet_pp", "blood_splat3", 192, 64, 64, 64, textureScale: 2));
            AddTexture(content, "crosshair", "Images/crosshair");

            //Shader1 = content.Load<Effect>("Shaders/Shader1");
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
