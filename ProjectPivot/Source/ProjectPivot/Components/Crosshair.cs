using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Entities;
using ProjectPivot.Utils;
using FarseerPhysics;

namespace ProjectPivot.Components {
    public class Crosshair : Component {
        public Vector2 Position { get; protected set; }
        public Vector2 WorldPosition { get; protected set; }
        public Vector2 spriteCenter = new Vector2(16, 16);
        public Cell HoverCell;
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
            HoverCell = Map.Current.CellAtWorld(WorldPosition);
            RaycastHit hit = PhysicsTools.RaycastFirst(Player.Current.Position, WorldPosition);
            if (hit != null) {
                Gizmo.Line(Player.Current.Position, hit.Point, Color.Azure);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Draw(Textures.Texture("crosshair"),
                WorldPosition,
                new Rectangle(0, 0, 32, 32),
                Color.White, 0f,
                spriteCenter,
                Vector2.One / Camera.Main.Zoom,
                SpriteEffects.None,
                0f);
        }
    }
}
