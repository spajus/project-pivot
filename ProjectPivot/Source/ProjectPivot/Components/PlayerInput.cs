using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class PlayerInput : Component {
        float speed = 200f;
        PlayerBody playerBody;

        public override void Initialize() {
            playerBody = GameObject.GetComponent<PlayerBody>();
        }
        public override void Update(GameTime gameTime) {
			float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
			float newX = 0f; //GameObject.Position.X;
			float newY = 0f; //GameObject.Position.Y;
			bool changed = false;
            if (keyboardState.IsKeyDown(Keys.A)) {
				newX -= speed * deltaTime;
				changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.D)) {
				newX += speed * deltaTime;
				changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.W)) {
				newY -= speed * deltaTime;
				changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.S)) {
				newY += speed * deltaTime;
				changed = true;
            }
			if (changed) {
				playerBody.Body.LinearVelocity = new Vector2(newX, newY);
			} else {
				playerBody.Body.LinearVelocity = Vector2.Zero;
			}
        }
    }
}
