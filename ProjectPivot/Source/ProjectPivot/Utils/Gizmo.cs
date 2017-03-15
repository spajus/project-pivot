using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectPivot.Utils {
	public static class Gizmo {
        class GizmoText {
            Vector2 position;
            string text;
            public GizmoText(string what, Vector2 position) {
                this.position = position;
                this.text = what;
            }
            public void Draw(SpriteBatch sb, SpriteFont font) {
                sb.DrawString(font, text, position, Color.Red);


            }
        }
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
        public static SpriteFont Font;
		public static Texture2D Pixel;

		static Queue<GizmoLine> lines = new Queue<GizmoLine>();
		static Queue<GizmoText> texts = new Queue<GizmoText>();

		public static void Initialize(GraphicsDevice graphics) {
			Pixel = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
			Pixel.SetData(new[] { Color.White });
		}

        public static void LoadContent(ContentManager content) {
            Font = content.Load<SpriteFont>("Fonts/debugText");
        }

		public static void Line(Vector2 from, Vector2 to) {
			lines.Enqueue(new GizmoLine(from, to));
		}

        public static void Text(string what, Vector2 position) {
            texts.Enqueue(new GizmoText(what, position));
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
			while (texts.Count > 0) {
				texts.Dequeue().Draw(spriteBatch, Font);
			}
		}

	}
}
