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
        public delegate void OnClickHandler(UIElement element);
        public string Id { get; protected set; }
        Color bgColor;
        Rectangle position;
        string text;

        public UIElement(string id, string text, Rectangle position) {
            this.Id = id;
            this.text = text;
            this.position = position;
        }

        public void Update(GameTime gameTime) {
            MouseState mState = Mouse.GetState();
            if (position.Contains(mState.Position)) {
                bgColor = Color.LightYellow;
                if (mState.LeftButton == ButtonState.Pressed) {
                    OnClick(this);
                }
            } else {
                bgColor = Color.LightGray;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Gizmo.Pixel, position, bgColor);
            spriteBatch.DrawString(Gizmo.Font, text, position.Center.ToVector2(), Color.Black);
        }
    }
}
