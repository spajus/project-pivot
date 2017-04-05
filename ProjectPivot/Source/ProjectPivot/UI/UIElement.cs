using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectPivot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.UI {
    public class UIElement {
        public event OnClickHandler OnClick;
        public delegate bool OnClickHandler(UIElement element);
        public string Id { get; protected set; }
        Color bgColor;
        Rectangle position;
        string text;
        bool IsHovering = false;

        public UIElement(string id, string text, Rectangle position) {
            this.Id = id;
            this.text = text;
            this.position = position;
        }

        public void Update(GameTime gameTime) {
            MouseState mState = Mouse.GetState();
            if (position.Contains(mState.Position)) {
                if (!IsHovering) {
                    Sounds.PlayEffect("bleep01", 50f);
                }
                bgColor = Color.LightYellow;
                if (mState.LeftButton == ButtonState.Pressed) {
                    if (OnClick(this)) {
                        Sounds.PlayEffect("bleep02", 50f);
                    }
                }
                IsHovering = true;
            } else {
                IsHovering = false;
                bgColor = Color.LightGray;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Gizmo.Pixel, position, bgColor);
            spriteBatch.DrawString(Gizmo.Font, text, position.Center.ToVector2(), Color.Black);
        }
    }
}
