using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPivot.Utils;
using ProjectPivot.Components;

namespace ProjectPivot.Entities {
    public class Weapon : GameObject {
        public GameObject Owner { get; set; } 
        public float Rotation { get; set; }
        public float RotationDeg { get { return MathHelper.ToDegrees(Rotation); } }
        public float CooldownTime = 250f;
        private float remainingCooldownTime = 0f;

        public Weapon(Vector2 position, GameObject owner = null) : base(position) {
            this.Owner = owner;
            AddComponent(new WeaponGraphics("sniper_rifle"));
        }

        protected override void OnUpdate(GameTime gameTime) {
            if (Owner != null) {
                Position = Owner.Position;
                if (Owner.GetComponent<PawnInput>().IsMoving) {
                    Position += new Vector2(
                        2* (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 10),
                        3*(float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 15));
                    }
                // todo sway
            }
            if (remainingCooldownTime > 0) {
                remainingCooldownTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            } 
        }

        public void Fire(Vector2 target) {
            if (remainingCooldownTime > 0) {
                return;
            } else {
                remainingCooldownTime = CooldownTime;
            }



            Bullet b = new Bullet(Owner, Position, target);
            GameObjects.Add(b, true);
        }
    }
}
