using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot;
using ProjectPivot.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    class PlayerGraphics : Component {
        PlayerInput input;
        float layerDepth = 0.2f;
        public override void Initialize() {
            input = GameObject.GetComponent<PlayerInput>();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            if (input.IsMoving) {
                if (input.RotationDeg <= 90 || input.RotationDeg > 270) {
                    Textures.Draw(spriteBatch, "player_left", GameObject.Position, layerDepth);
                } else {
                    Textures.Draw(spriteBatch, "player_left", GameObject.Position, layerDepth, 0, SpriteEffects.FlipHorizontally);
                }
            } else {
                if (input.RotationDeg > 180) {
                    Textures.Draw(spriteBatch, "player_down", GameObject.Position, layerDepth);
                } else {
                    Textures.Draw(spriteBatch, "player_up", GameObject.Position, layerDepth);
                }
            }

        }
    }
}
