using System;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPivot.Components.Items {
    public class ItemGraphics : Component {
        float depth = 0.4f;
        string sprite;
        public ItemGraphics(string id) {
            sprite = $"items/{id}";
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch, 
                          sprite, 
                          GameObject.Position, 
                          depth, 
                          0, 
                          SpriteEffects.None);
        }
    }
}
