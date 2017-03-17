using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ProjectPivot.Entities;

namespace ProjectPivot.Utils {
    public static class Gizmo {
        public static SpriteFont Font;
        public static Texture2D Pixel;

        static Queue<GizmoLine> lines = new Queue<GizmoLine>();
        static Queue<GizmoText> texts = new Queue<GizmoText>();
        static List<GizmoLine> permalines = new List<GizmoLine>();
        static List<GizmoText> permatexts = new List<GizmoText>();

        public static void Initialize(GraphicsDevice graphics) {
            Pixel = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.White });
        }

        public static void LoadContent(ContentManager content) {
            Font = content.Load<SpriteFont>("Fonts/debugText");
        }

        public static void Line(Vector2 from, Vector2 to, Color color, bool permanent = false) {
            if (!ProjectPivot.gizmosEnabled) { return; }
            if (permanent) {
                permalines.Add(new GizmoLine(from, to, color));
            } else {
                lines.Enqueue(new GizmoLine(from, to, color));
            }
        }

        public static void Text(string what, Vector2 position, Color color, bool permanent = false) {
            if (!ProjectPivot.gizmosEnabled) { return; }
            if (permanent) {
                permatexts.Add(new GizmoText(what, position, color));
            } else {
                texts.Enqueue(new GizmoText(what, position, color));
            }
        }

        public static void Rectangle(Rectangle rect, Color color, bool permanent = false) {
            if (!ProjectPivot.gizmosEnabled) { return; }
            Line(
                new Vector2(rect.Left, rect.Top),
                new Vector2(rect.Right, rect.Top), color, permanent);
            Line(
                new Vector2(rect.Left, rect.Bottom),
                new Vector2(rect.Right, rect.Bottom), color, permanent);
            Line(
                new Vector2(rect.Left, rect.Top),
                new Vector2(rect.Left, rect.Bottom), color, permanent);
            Line(
                new Vector2(rect.Right, rect.Top),
                new Vector2(rect.Right, rect.Bottom), color, permanent);
        }

        public static void Draw(SpriteBatch spriteBatch, bool grid = true) {
            if (!ProjectPivot.gizmosEnabled) { return; }
            if (grid) {
                new GizmoText("(X)", Vector2.Zero, Color.WhiteSmoke).Draw(spriteBatch, Font);
                for (int x = -200 * 32; x < 200 * 32; x += 32) {
                    new GizmoLine(new Vector2(x, -200 * 32), new Vector2(x, 200 * 32), Color.LightGray, 1).Draw(spriteBatch, Pixel);
                }
                for (int y = -200 * 32; y < 200 * 32; y += 32) {
                    new GizmoLine(new Vector2(-200 * 32, y), new Vector2(200 * 32, y), Color.LightGray, 1).Draw(spriteBatch, Pixel);
                }
            }
            while (lines.Count > 0) {
                lines.Dequeue().Draw(spriteBatch, Pixel);
            }
            while (texts.Count > 0) {
                texts.Dequeue().Draw(spriteBatch, Font);
            }
            foreach (GizmoLine line in permalines) {
                line.Draw(spriteBatch, Pixel);
            }
            foreach (GizmoText text in permatexts) {
                text.Draw(spriteBatch, Font);
            }
        }

        #region GizmoText
        class GizmoText {
            Vector2 position;
            string text;
            Color color;
            public GizmoText(string what, Vector2 position, Color color) {
                this.position = position;
                this.text = what;
                this.color = color;
            }
            public void Draw(SpriteBatch sb, SpriteFont font) {
                Vector2 p;
                Vector2 scale = Vector2.One / Camera.Main.Zoom; // 12f;// * Camera.Main.Zoom;
                float layerDepth = 0.1f;

                p = position + new Vector2(-1, -1);
                sb.DrawString(font, text, p, Color.Black, 
                    0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
                p = position + new Vector2(1, -1);
                sb.DrawString(font, text, p, Color.Black, 
                    0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
                p = position + new Vector2(-1, 1);
                sb.DrawString(font, text, p, Color.Black, 
                    0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
                p = position + new Vector2(1, 1);
                sb.DrawString(font, text, p, Color.Black, 
                    0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
                p = position + new Vector2(1, 1);
                sb.DrawString(font, text, p, Color.Black, 
                    0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);

                sb.DrawString(font, text, position, color, 
                    0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            }
        }
        #endregion

        #region GizmoLine
        class GizmoLine {
            Vector2 start;
            Vector2 end;
            Color color;
            int thickness;
            public GizmoLine(Vector2 from, Vector2 to, Color color, int thickness = 3) {
                this.start = from;
                this.end = to;
                this.color = color;
                this.thickness = thickness;
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
                        thickness), //width of line, change this to make thicker line
                    null,
                    color, //colour of line
                    angle,     //angle of line (calulated above)
                    new Vector2(0, 0), // point in line about which to rotate
                    SpriteEffects.None,
                    0);
            }
        }
        #endregion
    }
}
