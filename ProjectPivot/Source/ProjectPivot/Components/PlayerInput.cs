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
        public static bool Enabled = false;
        private float digCooldownMs = 0f;

        public override void Update(GameTime gameTime) {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            if (!Enabled) {
                Enabled = true; // Enabled after first skip ( to avoid bullets shooting on menu click)
                return;
            }
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
            dig(mouseState, gameTime);
        }

        private void dig(MouseState mouseState, GameTime gameTime) {
            bool canDig = false;
            if (Camera.Main.Crosshair.HoverCell != null && 
                ((Player)GameObject).CurrentCell.IsNeighbour(Camera.Main.Crosshair.HoverCell)) {
                Gizmo.Rectangle(Camera.Main.Crosshair.HoverCell.Area, Color.WhiteSmoke);
                canDig = true;
            }

            if (mouseState.LeftButton == ButtonState.Pressed) {
                Weapon.Fire(Camera.Main.Crosshair.WorldPosition);
            }
            if (mouseState.RightButton == ButtonState.Pressed) {
                if (canDig && digCooldownMs <= 0f) {
                    Camera.Main.Crosshair.HoverCell.TakeDamage(25, GameObject);
                    digCooldownMs = 250f;
                }
            }
            if (digCooldownMs > 0f) {
                digCooldownMs -= gameTime.ElapsedGameTime.Milliseconds;
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
