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
    public class Player : GameObject, Damageable {
        public static Player Current;
        public float Speed = 200f;
        public Weapon Weapon { get; protected set; }
        private PlayerInput input;
        private PawnBody body;
        public Cell CurrentCell;
        Health health;

        public Player(Vector2 position) : base(position) {
            AddComponent(new PawnGraphics());
            body = AddComponent<PawnBody>(new PawnBody());
            input = AddComponent<PlayerInput>(new PlayerInput()); //must go after body
            health = AddComponent<Health>(new Health(100f));

        }

        public void TakeWeapon(Weapon weapon) {
            weapon.Owner = this;
            Weapon = weapon;
            weapon.Initialize();
            input.Weapon = weapon;
        }

        protected override void OnUpdate(GameTime gameTime) {
            CurrentCell = Map.Current.CellAtWorld(Position);
        }

        public override Body PhysicsBody() {
            return body.Body;
        }

        public bool TakeDamage(int damage, GameObject source) {
            health.Decrease(damage);
            if (health.Value <= 0f) {
                //Destroy();
            }
            return true;
        } 
    }
}
