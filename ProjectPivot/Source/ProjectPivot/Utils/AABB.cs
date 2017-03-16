using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Utils {
    // AxisAlignedBoundingBox
    public class AABB {
        public Vector2 Center { get; protected set; }
        public Vector2 HalfDimension { get; protected set; }
        public float DhX { get; protected set; }
        public float DhY { get; protected set; }
        public Color Color = Color.Green;
        public Rectangle origRect;

        public AABB(Rectangle rect)  {
            this.origRect = rect;

            Center = CenterOf(rect);
            HalfDimension = new Vector2(
                (Center.X + rect.Width / 2f),
                (Center.Y + rect.Height / 2f));
            InitDh();
        }
        public AABB(Rectangle rect, Color color)  {
            Color = color;
            this.origRect = rect;
            Center = CenterOf(rect);
            HalfDimension = new Vector2(
                (Center.X + rect.Width / 2f),
                (Center.Y + rect.Height / 2f));
            InitDh();
            Rectangle rekt2 = ToRectangle();
            if (rect != rekt2) {
                Console.WriteLine("noo");
            }
        }

        public AABB(Vector2 center, Vector2 halfDimension) {
            Center = center;
            HalfDimension = halfDimension;
            InitDh();
        }

        public AABB(float x, float y, float hx, float hy) {
            Center = new Vector2(x, y);
            HalfDimension = new Vector2(hx, hy);
            InitDh();
        }

        public Rectangle ToRectangle() {
            // FIXME broken
            return new Rectangle(
                (int) (Center.X - DhX),
                (int) (Center.Y - DhY),
                (int) DhX * 2, 
                (int) DhY * 2);
        }

        public override string ToString() {
            return $"AABB: #{Center} #{HalfDimension}";
        }

        public bool Contains(Point point) {
            return Contains(point.X, point.Y);
        }

        public bool Contains(Vector2 point) {
            return Contains(point.X, point.Y);
        }

        public bool Contains(float x, float y) {
            if (!(Center.X + DhX >= x)) {
                return false;
            }
            if (!(Center.X - DhX <= x)) {
                return false;
            }
            if (!(Center.Y + DhY >= y)) {
                return false;
            }
            if (!(Center.Y- DhY <= y)) {
                return false;
            }
            return true;
        }

        public bool Intersects(AABB other) {
            float ocX = other.Center.X;
            float ohX = other.HalfDimension.X;
            float odhX = Math.Abs(ohX - ocX);
            if (!(Center.X + DhX >= ocX - odhX)) {
                return false;
            }
            if (!(Center.X - DhX <= ocX + odhX)) {
                return false;
            }
            float ocY = other.Center.Y;
            float ohY = other.HalfDimension.Y;
            float odhY = Math.Abs(ohY - ocY);
            if (!(Center.Y + DhY >= ocY - odhY)) {
                return false;
            }
            if (!(Center.Y - DhY <= ocY + odhY)) {
                return false;
            }
            return true;
        }

        private void InitDh() {
            DhX = Math.Abs(HalfDimension.X - Center.X);
            DhY = Math.Abs(HalfDimension.Y - Center.Y);
            Gizmo.Rectangle(this.ToRectangle(), Color); 
            Gizmo.Text("O", Center, Color);
        }

        private Vector2 CenterOf(Rectangle rectangle) {
            return new Vector2(
                (rectangle.X) + rectangle.Width / 2f,
                (rectangle.Y) + rectangle.Height / 2f);
        }

    }
}
