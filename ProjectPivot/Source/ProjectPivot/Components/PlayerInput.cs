using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class PlayerInput : Component {
        float speed = 500f;
        PlayerBody playerBody;
        Vector2 velocity;

        public object ConvertUtils { get; private set; }

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
			if (changed) {
                velocity = new Vector2(newX, newY);
                velocity.Normalize();
                if (velocity.LengthSquared() > 0.5) {
                    playerBody.Body.ApplyLinearImpulse(velocity * deltaTime);
                } 
                //playerBody.Body.LinearVelocity = (new Vector2(newX, newY) * deltaTime);
            }
        }
    }
}
