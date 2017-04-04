using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Utils;

namespace ProjectPivot.Screens {
    public class BootScreen : GameScreen {
        Texture2D studioScreen;

        float screenTimeoutMs = 1000f;

        public override void Initialize(GraphicsDevice graphics) {
            Gizmo.Initialize(graphics);
        }

        public override void LoadContent(ContentManager content) {
            Gizmo.LoadContent(content);
            studioScreen = content.Load<Texture2D>("images/gh_studios");
        }

        public override GameScreen Update(GameTime gameTime) {
            screenTimeoutMs -= gameTime.ElapsedGameTime.Milliseconds;
            if (screenTimeoutMs <= 0f) {
                return GameScreen.Screens["mainmenu"];
            }
            return this;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            spriteBatch.Begin();
            spriteBatch.Draw(
                studioScreen, Vector2.Zero, 
                new Rectangle(0, 0, 1200, 800), Color.White);
            spriteBatch.End(); 
        }
    }
}
