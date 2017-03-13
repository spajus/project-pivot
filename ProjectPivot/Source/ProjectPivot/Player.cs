using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot {
    public class Player {
        public Vector2 Position;
        public float Speed = 200f;
        public void Draw(SpriteBatch spriteBatch) {
            Textures.Draw(spriteBatch, "player_down", Position);
        }
    }
}
