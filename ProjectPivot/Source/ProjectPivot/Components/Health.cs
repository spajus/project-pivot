using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components {
    public class Health : Component {
        public float Value { get; protected set; }

        public Health(float value) {
            Value = value;
        }

        public void Set(float value) {
            Value = value;
        }

        public void Increase(float amount) {
            Value += amount;
        }
        public void Decrease(float amount) {
            Value -= amount;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Gizmo.Text(Value.ToString(), GameObject.Position);
        }
    }
}
