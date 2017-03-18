using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    class Weapon : GameObject {
        public GameObject Owner { get; protected set; } 
        public float Rotation { get; set; }
        public float RotationDeg { get { return MathHelper.ToDegrees(Rotation); } }

        public Weapon(Vector2 position, GameObject owner = null) : base(position) {
            this.Owner = owner;
            AddComponent(new WeaponGraphics());
        }

        protected override void OnUpdate(GameTime gameTime) {
            if (Owner != null) {
                Position = Owner.Position;
                // todo sway
            }
        }
    }
}
