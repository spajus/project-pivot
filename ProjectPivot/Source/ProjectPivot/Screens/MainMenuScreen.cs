using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.UI;
using System.Collections.Generic;
using ProjectPivot.Entities;
using Microsoft.Xna.Framework.Input;
using ProjectPivot.Utils;

namespace ProjectPivot.Screens {
    public class MainMenuScreen : GameScreen {

        List<UIElement> buttons;
        GameScreen nextScreen = null;

        public override void Enter(GameScreen oldScreen) {
            ProjectPivot.Current.IsMouseVisible = true;
            base.Enter(oldScreen);
        }

        public override void Leave(GameScreen newScreen) {
            ProjectPivot.Current.IsMouseVisible = false;
            base.Leave(newScreen);
        }

        public override void Initialize(GraphicsDevice graphics) {
            buttons = new List<UIElement>();
            int i = 0;
            buttons.Add(new UIElement("btnContinue", "Continue", new Rectangle(0, i++ * 60, 200, 50)));
            buttons.Add(new UIElement("btnNewGame", "New Game", new Rectangle(0, i++ * 60, 200, 50)));
            buttons.Add(new UIElement("btnLoadGame", "Load Game", new Rectangle(0, i++ * 60, 200, 50)));
            buttons.Add(new UIElement("btnExit", "Exit", new Rectangle(0, i++ * 60, 200, 50)));
            buttons.ForEach(b => b.OnClick += uiClick);

            UserInput.OnKeyPressed += UserInput_OnKeyPressed;
        }

        private void UserInput_OnKeyPressed(Keys keys) {
            if (IsActive) {
                if (keys == Keys.Escape) {
                    if (GameWorld.Initialized) {
                        nextScreen = GameScreen.Screens["maingame"];
                    }
                }
            }
        }

        private bool uiClick(UIElement element) {
            switch (element.Id) {
                case "btnContinue": {
                        if (GameWorld.Initialized) {
                            nextScreen = GameScreen.Screens["maingame"];
                            return true;
                        }
                        break;
                    }
                case "btnNewGame": {
                        nextScreen = GameScreen.Screens["maingame"];
                        nextScreen.ResetState();
                        return true;
                        break;
                    }
                case "btnExit": {
                        // TODO save here
                        ProjectPivot.Current.Exit();
                        break;
                    }
            }
            return false;
        }

        public override GameScreen Update(GameTime gameTime) {
            if (nextScreen != null) {
                GameScreen toReturn = nextScreen;
                nextScreen = null;
                return toReturn;
            } else {
                buttons.ForEach(b => b.Update(gameTime));
                return this;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            spriteBatch.Begin();
            buttons.ForEach(b => b.Draw(spriteBatch));
            spriteBatch.End();
        }
    }
}
