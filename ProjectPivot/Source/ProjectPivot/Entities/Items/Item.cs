using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Components.Items;
using ProjectPivot.Utils;

namespace ProjectPivot.Entities.Items {
    public class Item : GameObject {
        public string Id;
        public string Name;
        public bool IsOnGround = true;

        protected override void OnInitialize() {
            AddComponent(new ItemGraphics(Id));
        }

        protected override void OnUpdate(GameTime gameTime) {
            if (Vector2.DistanceSquared(Player.Current.Position, Position) < 32 * 32) {
                Player.Current.Inventory.Pickup(this);
                Sounds.PlayEffect("bleep01", 50f);
            }
        }
    }
}
