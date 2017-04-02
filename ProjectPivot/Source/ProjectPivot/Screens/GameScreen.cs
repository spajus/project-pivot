using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPivot.Screens {
    public abstract class GameScreen {
        public static Dictionary<string, GameScreen> Screens = new Dictionary<string, GameScreen>();
        public static GameScreen Current = null;

        public static void InitializeScreens() {
            // Add all screens here
            Screens.Add("boot", new BootScreen());
            Screens.Add("mainmenu", new MainMenuScreen());

            foreach (GameScreen screen in Screens.Values) {
                screen.Initialize();
            }

            Current = Screens["boot"];
        }

        public static void LoadScreensContent(ContentManager content) {
            foreach (GameScreen screen in Screens.Values) {
                screen.LoadContent(content);
            }
        }

        public static void SwitchTo(GameScreen newScreen) {
            Current.Leave(newScreen);
            newScreen.Enter(Current);
            Current = newScreen;
        }

        public virtual void Enter(GameScreen oldScreen) { }
        public virtual void Leave(GameScreen newScreen) { }
        public virtual void Initialize() { }
        public virtual void LoadContent(ContentManager content) { }
        public virtual void UnloadContent() { }
        public virtual GameScreen Update(GameTime gameTime) { return this; }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) { }
    }
}
