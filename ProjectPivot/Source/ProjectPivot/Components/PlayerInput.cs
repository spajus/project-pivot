using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class PlayerInput : Component {
        float speed = 100f;
        public override void Update(GameTime gameTime) {
			float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            float newX = GameObject.Position.X;
            float newY = GameObject.Position.Y;
            if (keyboardState.IsKeyDown(Keys.A)) {
				newX -= speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.D)) {
				newX += speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.W)) {
				newY -= speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.S)) {
				newY += speed * deltaTime;
            }
            GameObject.Move(new Vector2(newX, newY));
        }
    }
}
