using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public class Player : GameObject {
        public Player(Vector2 position) : base(position) {
            AddComponent(new PlayerGraphics());
            AddComponent(new PlayerInput());
        }
        public float Speed = 200f;
    }
}
