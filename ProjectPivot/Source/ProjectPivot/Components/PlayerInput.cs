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
    public class PlayerInput : Component {
        float speed = 100f;
        PlayerBody playerBody;
        public Direction direction;
        public float Rotation = 0f;
        Vector2 velocity;
        const float maxSpeed = 5f;

        public object ConvertUtils { get; private set; }

        public override void Initialize() {
            playerBody = GameObject.GetComponent<PlayerBody>();
        }
        public override void Update(GameTime gameTime) {
            Gizmo.Rectangle(
                new Rectangle((int)GameObject.Position.X, (int)GameObject.Position.Y, 32, 32), Color.Beige);
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
                    if (playerBody.Body.LinearVelocity.LengthSquared() < maxSpeed) {
                        playerBody.Body.LinearDamping = 0.00f;
                    } else {
                        playerBody.Body.LinearDamping = 3f;
                    }
                    playerBody.Body.ApplyLinearImpulse(velocity * deltaTime);
                }
                //playerBody.Body.LinearVelocity = (new Vector2(newX, newY) * deltaTime);
            } else {
                //playerBody.Body.LinearVelocity = Vector2.Zero;
                playerBody.Body.LinearDamping = 10f;
            }
            changeDirection();
        }

        void changeDirection() {
            Vector2 xPos = Camera.Main.Crosshair.WorldPosition;
            Rotation = (float) Math.Atan2(xPos.Y - GameObject.Position.Y,
                xPos.X - GameObject.Position.X);
            Gizmo.Text($"Angle: {Rotation}", 
                GameObject.Position, Color.Yellow);
            
        }
    }
}
