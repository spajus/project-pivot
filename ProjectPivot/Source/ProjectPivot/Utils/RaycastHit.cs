using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Utils {
    public class RaycastHit {
        public Fixture Fixture { get; protected set; }
        public Vector2 Point { get; protected set; }
        public Vector2 Normal { get; protected set; }

        public RaycastHit(Fixture fixture, Vector2 point, Vector2 normal) {
            Fixture = fixture;
            Point = ConvertUnits.ToDisplayUnits(point);
            Normal = ConvertUnits.ToDisplayUnits(normal);
        }
    }
}
