using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Screens;

namespace ProjectPivot {
    public class ProjectPivot : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int screenWidth = 1200;
        public const int screenHeight = 800;

        public ProjectPivot() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
        }

        protected override void Initialize() {
            GameScreen.InitializeScreens();
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameScreen.LoadScreensContent(Content);
            base.LoadContent();
        }

        protected override void UnloadContent() {
            // figure out what to do here
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime) {
            GameScreen newScreen = GameScreen.Current.Update(gameTime);
            if (newScreen != GameScreen.Current) {
                GameScreen.SwitchTo(newScreen);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);
            GameScreen.Current.Draw(gameTime, spriteBatch, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}
