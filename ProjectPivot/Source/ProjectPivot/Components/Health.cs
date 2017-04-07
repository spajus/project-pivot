using Microsoft.Xna.Framework;
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
        public bool IsHealthy { get { return Value >= 40; } }

        public static bool IsGameObjectHealthy(float health) {
            return health >= 40;
        }

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
            // Make some objects invinsibruuuu!
            if (Value < 150) {
                Value -= amount;
            }
        }

        public void DrawHealth() {
            Gizmo.Text(Value.ToString(), GameObject.Position, Color.Green);
        }
    }
}
