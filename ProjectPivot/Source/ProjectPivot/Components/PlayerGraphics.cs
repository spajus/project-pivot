using Microsoft.Xna.Framework.Graphics;
using ProjectPivot;
using ProjectPivot.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    class PlayerGraphics : Component {
        public override void Draw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch, "player_down", GameObject.Position);
        }
    }
}
