using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Utils;

namespace ProjectPivot.Entities {
    public class TempText : GameObject {
        private float scale;
        private float lifetimeMillis;
        private string text;
        private Color color;

        public static void Write(
            Vector2 position, string text, Color color, float lifetimeMillis, float initialScale) {
            GameObjects.Add(new TempText(position, text, color, lifetimeMillis, initialScale), true);
        }

        public TempText(Vector2 position, string text, Color color, float lifetimeMillis, float initialScale) : base(position) {
            scale = initialScale;
            this.lifetimeMillis = lifetimeMillis;
            this.color = color;
            this.text = text;
        }

        protected override void OnUpdate(GameTime gameTime) {
            lifetimeMillis -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            if (lifetimeMillis <= 0) {
                Destroy();
            }
            scale -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void OnDraw(SpriteBatch spriteBatch) {
            Vector2 origin = Gizmo.Font.MeasureString(text) / 2;

            spriteBatch.DrawString(Gizmo.Font,
                                   text,
                                   Position + new Vector2(-1, -1),
                                   Color.Black, 0,
                                   origin,
                                   scale, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Gizmo.Font,
                                   text,
                                   Position + new Vector2(1, 1),
                                   Color.Black, 0,
                                   origin,
                                   scale, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Gizmo.Font,
                                   text,
                                   Position + new Vector2(1, -1),
                                   Color.Black, 0,
                                   origin,
                                   scale, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Gizmo.Font,
                                   text,
                                   Position + new Vector2(-1, 1),
                                   Color.Black, 0,
                                   origin,
                                   scale, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Gizmo.Font,
                                   text,
                                   Position,
                                   color, 0,
                                   origin,
                                   scale, SpriteEffects.None, 0.15f);

        }
    }
}
