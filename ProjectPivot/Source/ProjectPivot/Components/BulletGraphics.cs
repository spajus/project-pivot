using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Components {
    class BulletGraphics : Component {
        static Vector2 offset = new Vector2(1, 0);
        public override void Draw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch, "small_bullet", GameObject.Position + offset, 0.05f);
        }
    }
}
