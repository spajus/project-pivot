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
            float gunDepth = 0.1f;
            SpriteEffects gunSfx = SpriteEffects.None;
            float gunAngleAdjustment = 0.0f;
            Vector2 gunOffset = new Vector2(8, 4);


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
                    gunDepth = 0.3f;
                    if (input.IsMoving) {
                        gunOffset = new Vector2(8, -4);
                    }
                }
            }

            // gun adj
            if (input.RotationDeg <= 90 || input.RotationDeg > 270) {
                gunSfx = SpriteEffects.FlipHorizontally;
                gunAngleAdjustment = MathHelper.ToRadians(90);
                gunOffset = new Vector2(-8, 4);
            }

            Textures.Draw(spriteBatch, "sniper_rifle", GameObject.Position + gunOffset, 
                gunDepth, input.Rotation + gunAngleAdjustment, gunSfx);
        }
    }
}
