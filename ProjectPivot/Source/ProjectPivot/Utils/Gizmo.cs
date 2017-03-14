using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ProjectPivot.Utils {
	public static class Gizmo {
        class GizmoLine {
            Vector2 start;
            Vector2 end;
            public GizmoLine(Vector2 from, Vector2 to) {
                this.start = from;
                this.end = to;
            }
            public void Draw(SpriteBatch sb, Texture2D pixel) {
                Vector2 edge = end - start;
                // calculate angle to rotate line
                float angle = (float)Math.Atan2(edge.Y, edge.X);

                sb.Draw(pixel,
                    new Rectangle(// rectangle defines shape of line and position of start of line
                        (int)start.X,
                        (int)start.Y,
                        (int)edge.Length(), //sb will strech the texture to fill this rectangle
                        3), //width of line, change this to make thicker line
                    null,
                    Color.Red, //colour of line
                    angle,     //angle of line (calulated above)
                    new Vector2(0, 0), // point in line about which to rotate
                    SpriteEffects.None,
                    0);
            }
        }
		public static Texture2D Pixel;
		static Queue<GizmoLine> lines = new Queue<GizmoLine>();

		public static void Initialize(GraphicsDevice graphics) {
			Pixel = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
			Pixel.SetData(new[] { Color.White });
		}

		public static void Line(Vector2 from, Vector2 to) {
			lines.Enqueue(new GizmoLine(from, to));
		}

		public static void Rectangle(Rectangle rect) {
			Line(
				new Vector2(rect.Left, rect.Top),
				new Vector2(rect.Right, rect.Top));
			Line(
				new Vector2(rect.Left, rect.Bottom),
				new Vector2(rect.Right, rect.Bottom));
			Line(
				new Vector2(rect.Left, rect.Top),
				new Vector2(rect.Left, rect.Bottom));
			Line(
				new Vector2(rect.Right, rect.Top),
				new Vector2(rect.Right, rect.Bottom));
		}

		public static void Draw(SpriteBatch spriteBatch) {
			while (lines.Count > 0) {
				lines.Dequeue().Draw(spriteBatch, Pixel);
			}
		}
	}
}
