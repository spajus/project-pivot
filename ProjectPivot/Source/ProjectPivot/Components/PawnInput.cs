using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Components;
using ProjectPivot.Entities;
using ProjectPivot.Utils;

namespace ProjectPivot.Components {
    public class PawnInput : Component {
        public const float maxSpeed = 5f;
        public float speed = 100f;
        protected PawnBody pawnBody;
        public Weapon Weapon;
        public Direction direction;
        public float Rotation = 0f;
        // 0 deg = 9 o'clock
        public float RotationDeg { get { return MathHelper.ToDegrees(Rotation) + 180; } }
        public bool IsMoving;
        protected Vector2 velocity;

        public override void Initialize() {
            pawnBody = GameObject.GetComponent<PawnBody>();
        }

        protected void ApplyVelocity(GameTime gameTime, float newX, float newY, bool changed = true) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            IsMoving = false;
            if (changed) {
                velocity = new Vector2(newX, newY);
                velocity.Normalize();

                if (velocity.LengthSquared() > 0.5) {
                    if (pawnBody.Body.LinearVelocity.LengthSquared() < maxSpeed) {
                        pawnBody.Body.LinearDamping = 0.00f;
                    } else {
                        pawnBody.Body.LinearDamping = 3f;
                    }
                    pawnBody.Body.ApplyLinearImpulse(velocity * deltaTime);
                    IsMoving = true;
                }
                //playerBody.Body.LinearVelocity = (new Vector2(newX, newY) * deltaTime);
            } else {
                //playerBody.Body.LinearVelocity = Vector2.Zero;
                pawnBody.Body.LinearDamping = 10f;
            }
        }
    }
}
