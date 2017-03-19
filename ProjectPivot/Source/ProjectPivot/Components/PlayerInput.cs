using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectPivot.Entities;
using ProjectPivot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class PlayerInput : PawnInput {
        public override void Update(GameTime gameTime) {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
			float newX = 0f; //GameObject.Position.X;
			float newY = 0f; //GameObject.Position.Y;
			bool changed = false;
            if (keyboardState.IsKeyDown(Keys.A)) {
				newX -= speed;
				changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.D)) {
				newX += speed;
				changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.W)) {
				newY -= speed;
				changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.S)) {
				newY += speed;
				changed = true;
            }

            ApplyVelocity(gameTime, newX, newY, changed);

            changeDirection();

            if (mouseState.LeftButton == ButtonState.Pressed) {
                Weapon.Fire(Camera.Main.Crosshair.WorldPosition);
            }
        }

        void changeDirection() {
            Vector2 xPos = Camera.Main.Crosshair.WorldPosition;
            Rotation = (float) Math.Atan2(xPos.Y - GameObject.Position.Y,
                xPos.X - GameObject.Position.X);
            //Gizmo.Text(RotationDeg.ToString(), xPos, Color.White);
        }
    }
}
