using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot {
    public class Input {
        private Player player;
        private Camera camera;

        public Input(Player player, Camera camera) {
            this.player = player;
            this.camera = camera;
        }

        public void Update(GameTime gameTime) {
			float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A)) {
				player.Position.X -= player.Speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.D)) {
				player.Position.X += player.Speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.W)) {
				player.Position.Y -= player.Speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.S)) {
				player.Position.Y += player.Speed * deltaTime;
            }
        }
    }
}
