using System;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Utils {
    public static class MathTools {
        public static Vector2 AngleToVector2(float angleInRadians) {
            return new Vector2(
                (float)Math.Cos(angleInRadians), 
                -(float)Math.Sin(angleInRadians));
        }
    }

}
