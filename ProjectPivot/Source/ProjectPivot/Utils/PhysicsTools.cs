using System;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace ProjectPivot.Utils {
    public static class PhysicsTools {
        public static RaycastHit RaycastFirst(Vector2 from, Vector2 to) {
            RaycastHit hit = null;
            Func<Fixture, Vector2, Vector2, float, float> cb = delegate (
                    Fixture f, Vector2 point, Vector2 normal, float fraction) {
                hit = new RaycastHit(f, point, normal);
                return fraction;
            };

            // Summary:
            //     Ray-cast the world for all fixtures in the path of the ray. Your callback
            //     controls whether you get the closest point, any point, or n-points.  The
            //     ray-cast ignores shapes that contain the starting point.  Inside the callback:
            //     return -1: ignore this fixture and continue 
            //     return  0: terminate the ray cast
            //     return fraction: clip the ray to this point
            //     return 1:        don't clip the ray and continue
            from = ConvertUnits.ToSimUnits(from);
            to = ConvertUnits.ToSimUnits(to);
            ProjectPivot.World.RayCast(cb, from, to);
            return hit;
        }
    }
}
