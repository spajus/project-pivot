using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Entities;
using ProjectPivot.Utils;

namespace ProjectPivot.Components {
    public class Crosshair : Component {
        public Vector2 Position { get; protected set; }
        public Vector2 WorldPosition { get; protected set; }
        public Vector2 spriteCenter = new Vector2(16, 16);
        private Camera camera;
        public Crosshair() {

        }

        public override void Initialize() {
            camera = GameObject as Camera;
        }

        public override void Update(GameTime gameTime) {
            MouseState mouse = Mouse.GetState();
            Position = new Vector2(mouse.Position.X, mouse.Position.Y);
            WorldPosition = Camera.Main.ToWorldCoordinates(Position);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Textures.Texture("crosshair"),
                WorldPosition - spriteCenter / Camera.Main.Zoom,
                new Rectangle(0, 0, 32, 32),
                Color.White, 0f,
                Vector2.Zero,
                Vector2.One / Camera.Main.Zoom,
                SpriteEffects.None,
                0f);

        }

    }
}
