using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;

namespace ProjectPivot.Entities {
    public class Player : GameObject {
        public float Speed = 200f;
        public Weapon Weapon { get; protected set; }
        private PlayerInput input;
        private PlayerBody body;

        public Player(Vector2 position) : base(position) {
            AddComponent(new PlayerGraphics());
            body = AddComponent<PlayerBody>(new PlayerBody());
            input = AddComponent<PlayerInput>(new PlayerInput()); //must go after body
        }

        public void TakeWeapon(Weapon weapon) {
            weapon.Owner = this;
            Weapon = weapon;
            weapon.Initialize();
            input.Weapon = weapon;
        }

        public override Body PhysicsBody() {
            return body.Body;
        }

    }
}
