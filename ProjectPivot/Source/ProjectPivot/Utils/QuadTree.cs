using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Utils {
    public class QuadTree {
        private const int NODE_CAPACITY = 24;
        private QuadTree ne, nw, se, sw;
        List<GameObject> gameObjects;
        AABB boundary;

        public QuadTree(Rectangle boundry) : this(new AABB(boundry)) { }

        public QuadTree(AABB boundary) {
            this.boundary = boundary;
            //Gizmo.Rectangle(boundary.ToRectangle(), Color.Yellow, true);
            this.gameObjects = new List<GameObject>();
        }

        public override string ToString() {
            return $"QuadTree: {boundary} GOs: {gameObjects.Count}";
        }

        public bool Insert(GameObject gameObject) {
            if (!boundary.Contains(gameObject.Position)) {
                return false;
            }
            if (gameObjects.Count < NODE_CAPACITY) {
                gameObjects.Add(gameObject);
                return true;
            }

            if (nw == null) {
                subdivide();
            }

            if (nw.Insert(gameObject)) { return true; }
            if (ne.Insert(gameObject)) { return true; }
            if (sw.Insert(gameObject)) { return true; }
            if (se.Insert(gameObject)) { return true; }

            throw new Exception("QuadTree failed to insert");
        }

        public bool Remove(GameObject gameObject) {
            if (!boundary.Contains(gameObject.Position)) {
                return false;
            }
            if (gameObjects.Remove(gameObject)) {
                return true;
            }
            if (nw == null) {
                return false;
            }
            if (nw.Remove(gameObject)) { return true; }
            if (ne.Remove(gameObject)) { return true; }
            if (sw.Remove(gameObject)) { return true; }
            if (se.Remove(gameObject)) { return true; }
            return false;
        }

        public List<GameObject> QueryRange(AABB range) {
            List<GameObject> result = new List<GameObject>();
            if (!boundary.Intersects(range)) {
                return new List<GameObject>();
            }
            foreach (GameObject go in gameObjects) {
                if (range.Contains(go.Position)) {
                    result.Add(go);
                }
            }
            if (ne == null) {
                // not subdivided
                return result;
            }
            result.AddRange(nw.QueryRange(range));
            result.AddRange(ne.QueryRange(range));
            result.AddRange(sw.QueryRange(range));
            result.AddRange(se.QueryRange(range));

            return result;
        }

        private void subdivide() {
            float cx = boundary.Center.X;
            float cy = boundary.Center.Y;
            float hx = boundary.HalfDimension.X;
            float hy = boundary.HalfDimension.Y;
            float hhx = Math.Abs(cx - hx) / 2.0f;
            float hhy = Math.Abs(cy - hy) / 2.0f;
            nw = new QuadTree(new AABB(cx - hhx, cy - hhy, cx, cy));
            ne = new QuadTree(new AABB(cx + hhx, cy - hhy, cx, cy));
            sw = new QuadTree(new AABB(cx - hhx, cy + hhy, cx, cy));
            se = new QuadTree(new AABB(cx + hhx, cy + hhy, cx, cy));
        }

    }
}
