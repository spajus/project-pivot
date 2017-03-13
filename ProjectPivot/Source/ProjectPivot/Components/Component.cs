using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class Component {

        public GameObject GameObject;

        public virtual void Update(GameTime gameTime) {
            
        }
        public virtual void Draw(SpriteBatch spriteBatch) {

        }
    }
}
