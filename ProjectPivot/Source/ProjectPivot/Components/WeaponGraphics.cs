using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;
namespace ProjectPivot.Entities {
    public class WeaponGraphics : Component {
        private GameObject owner;
        private PawnInput input;
        public string sprite;

        public WeaponGraphics() {

        }

        public WeaponGraphics(string sprite) {
            this.sprite = sprite;
        }
        public override void Initialize() {
            owner = ((Weapon) GameObject).Owner;
            input = owner.GetComponent<PawnInput>();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            SpriteEffects gunSfx = SpriteEffects.None;
            float gunAngleAdjustment = 0.0f;
            if (input == null) {
                Textures.Draw(
                    spriteBatch, sprite,
                    GameObject.Position,
                    0.3f, // behind player, on ground
                    gunAngleAdjustment,
                    gunSfx);
                return;

            }

            float gunDepth = 0.1f; //in front of player
            Vector2 gunOffset = new Vector2(8, 4);

            if (!input.IsMoving) {
                if (input.RotationDeg <= 180) {
                    gunDepth = 0.3f;
                    gunOffset = new Vector2(8, -6);
                }
            }

            // gun adj
            if (input.RotationDeg <= 90 || input.RotationDeg > 270) {
                gunSfx = SpriteEffects.FlipHorizontally;
                gunAngleAdjustment = MathHelper.ToRadians(90);
                if (input.RotationDeg <= 90) {
                    // aiming top-left
                    if (input.IsMoving) {
                        gunOffset = new Vector2(-8, 4);
                    } else {
                        gunOffset = new Vector2(-8, -4);
                    }
                } else {
                    //aiming top-right
                    gunOffset = new Vector2(-8, 4);

                }
            }

            Textures.Draw(spriteBatch, "sniper_rifle", GameObject.Position + gunOffset,
                gunDepth, input.Rotation + gunAngleAdjustment, gunSfx);
        }
    }
}
