using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Entities;
using ProjectPivot.Components;
using Microsoft.Xna.Framework.Input;
using ProjectPivot.Utils;

namespace ProjectPivot.Screens {
    public class MainGameScreen : GameScreen {

        PhysicsDebug physicsDebug;
        FPSCounter fpsCounter;

        bool isPaused = false;

        public override void Initialize(GraphicsDevice graphics) {
            UserInput.OnKeyPressed += UserInput_OnKeyPressed;
            fpsCounter = new FPSCounter();

        }

        private void UserInput_OnKeyPressed(Keys keys) {
            if (IsActive) {
                if (keys == Keys.Escape) {
                    isPaused = true;
                }

            }
        }

        public override void ResetState() {
            GameWorld.Current = new GameWorld();
            GameWorld.Current.Initialize();
        }

        public override void LoadContent(ContentManager content) {
            Textures.LoadContent(content);
            CellGraphics.LoadContent(content);
            fpsCounter.LoadContent(content);
        }

        public override void Enter(GameScreen oldScreen) {
            Settings.BACKGROUND_COLOR = Color.Black;
            if (!GameWorld.Initialized) {
                ResetState();
            }
            base.Enter(oldScreen);
        }

        public override void Leave(GameScreen newScreen) {
            Settings.BACKGROUND_COLOR = Color.White;
            PlayerInput.Enabled = false;
            base.Leave(newScreen);
        }

        public override GameScreen Update(GameTime gameTime) {
            if (isPaused) {
                isPaused = false;
                return Screens["mainmenu"];
            }
            // variable time step but never less then 30 Hz
            double frameTime = gameTime.ElapsedGameTime.TotalSeconds;
            GameWorld.Current.World.Step((float)Math.Min(frameTime, Settings.MIN_PHYSICS_STEP_TIME));
            GameWorld.Current.Update(gameTime);
            return base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
			// TODO: Add your drawing code here
            // TODO: bloom map
			spriteBatch.Begin(SpriteSortMode.BackToFront,
                              BlendState.AlphaBlend,
                              Settings.SAMPLER_STATE,
                              DepthStencilState.Default,
                              RasterizerState.CullNone,
                              Settings.GLOBAL_SHADER,
                              Camera.Main.Transform);
            GameWorld.Current.Map.Draw(spriteBatch);
            spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.BackToFront,
                              BlendState.AlphaBlend,
                              Settings.SAMPLER_STATE,
                              DepthStencilState.Default,
                              RasterizerState.CullNone,
                              Settings.GLOBAL_SHADER,
                              Camera.Main.Transform);
            GameObjects.Draw(spriteBatch);
            spriteBatch.End();

            // GIZMOS
			spriteBatch.Begin(SpriteSortMode.BackToFront,
                              BlendState.AlphaBlend,
                              Settings.SAMPLER_STATE,
                              DepthStencilState.Default,
                              RasterizerState.CullNone,
                              Settings.GLOBAL_SHADER,
                              Camera.Main.Transform);
            GameObjects.Draw(spriteBatch);
            Gizmo.Draw(spriteBatch, Settings.DEBUG_GRID);

            fpsCounter.Update(gameTime);
            fpsCounter.Draw(spriteBatch, Camera.Main);

            spriteBatch.End();

            if (Settings.PHYSICS_DEBUG) {
                if (physicsDebug == null) {
                    physicsDebug = new PhysicsDebug(GameWorld.Current.World);
                    physicsDebug.LoadContent(
                        ProjectPivot.Current.GraphicsDevice, 
                        ProjectPivot.Current.Content);
                }
                physicsDebug.Draw();
            } 

            base.Draw(gameTime, spriteBatch, graphicsDevice);
        }
    }
}
