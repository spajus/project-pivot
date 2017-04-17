using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Entities;
using ProjectPivot.Screens;
using ProjectPivot.Utils;

namespace ProjectPivot {
    public class ProjectPivot : Game {

        public static ProjectPivot Current;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public ProjectPivot() {
            Current = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Settings.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Settings.SCREEN_HEIGHT;
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
        }

        protected override void Initialize() {
            GameScreen.InitializeScreens(GraphicsDevice);
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
            UserInput.Update(gameTime);
            Sounds.Update(gameTime);
            GameScreen newScreen = GameScreen.Current.Update(gameTime);
            if (newScreen != GameScreen.Current) {
                GameScreen.SwitchTo(newScreen);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Settings.BACKGROUND_COLOR);
            GameScreen.Current.Draw(gameTime, spriteBatch, GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
