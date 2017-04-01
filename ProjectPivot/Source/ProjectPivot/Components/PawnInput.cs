using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Components;
using ProjectPivot.Entities;
using ProjectPivot.Utils;

namespace ProjectPivot.Components {
    public class PawnInput : Component {
        public float maxSpeed = 5f;
        public float speed = 100f;
        public PawnBody PawnBody;
        public Weapon Weapon;
        public Direction Direction;
        public float Rotation = 0f;
        // 0 deg = 9 o'clock
        public float RotationDeg { get { return MathHelper.ToDegrees(Rotation) + 180; } }
        public bool IsMoving;
        protected Vector2 velocity;

        public override void Initialize() {
            PawnBody = GameObject.GetComponent<PawnBody>();
        }

        protected void ApplyVelocity(GameTime gameTime, float newX, float newY, bool changed = true, bool normalize = true) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            IsMoving = false;
            if (changed) {
                velocity = new Vector2(newX, newY);
                if (normalize) {
                    velocity.Normalize();
                }

                if (velocity.LengthSquared() > 0.5) {
                    if (PawnBody.Body.LinearVelocity.LengthSquared() < maxSpeed) {
                        PawnBody.Body.LinearDamping = 0.00f;
                    } else {
                        PawnBody.Body.LinearDamping = 3f;
                    }
                    PawnBody.Body.ApplyLinearImpulse(velocity * deltaTime);
                    IsMoving = true;
                }
                //playerBody.Body.LinearVelocity = (new Vector2(newX, newY) * deltaTime);
            } else {
                //playerBody.Body.LinearVelocity = Vector2.Zero;
                PawnBody.Body.LinearDamping = 10f;
            }
        }
    }
}
