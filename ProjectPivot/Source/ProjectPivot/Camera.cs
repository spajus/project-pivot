using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot {
    public class Camera {

        #region Fields
        public float Zoom;
        public Matrix Transform;
        public Matrix InverseTransform { get; protected set; }
        public Vector2 Position;
        public Vector2 WorldPosition { get; protected set; }
        public Rectangle VisibleArea { get; protected set; }
        public float Rotation;
        private Viewport viewport;
        private MouseState mouseState;
        private Int32 prevMouseScrollValue;
		private float cameraSpeed = 500f;
		private float zoomSpeed = 3f;
        #endregion

        #region Constructor
        public Camera(Viewport viewport) {
            this.Zoom = 1.0f;
            this.Rotation = 0.0f;
            this.Position = Vector2.Zero;
            this.viewport = viewport;
        }
        #endregion

        #region Public Methods
        public Vector2 MouseWorldCoordinates() {
            return ToWorldCoordinates(new Vector2(mouseState.X, mouseState.Y));
        }

        public bool IsVisible(Vector2 position) {
            return VisibleArea.Contains(position);
        }

        public bool IsVisible(Rectangle position) {
            return VisibleArea.Contains(position);
        }

        public Vector2 ToWorldCoordinates(Vector2 screenPosition) {
            return Vector2.Transform(screenPosition, InverseTransform);
        }

        public void Update(GameTime gameTime) {
            ReactToUserInput(gameTime);
            WorldPosition = ToWorldCoordinates(Position);
            VisibleArea = CalculateVisibleArea();
            Zoom = MathHelper.Clamp(Zoom, 0.01f, 10.0f);
            // Rotation = ClampRotation();
            Transform = Matrix.CreateRotationZ(Rotation) *
                 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                 Matrix.CreateTranslation(Position.X, Position.Y, 0);
            InverseTransform = Matrix.Invert(Transform);
        }

        public void ReactToUserInput(GameTime gameTime) {
			float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            mouseState = Mouse.GetState();
            if (mouseState.ScrollWheelValue > prevMouseScrollValue) {
                Zoom += zoomSpeed * deltaTime;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
            } else if (mouseState.ScrollWheelValue < prevMouseScrollValue) {
                Zoom -= zoomSpeed * deltaTime;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
            }
        }
        #endregion

        #region Protected Functions
        Rectangle CalculateVisibleArea() {
            var tl = Vector2.Transform(Vector2.Zero, InverseTransform);
            var tr = Vector2.Transform(new Vector2(viewport.Width / Zoom, 0), InverseTransform);
            var bl = Vector2.Transform(new Vector2(0, viewport.Height / Zoom), InverseTransform);
            var br = Vector2.Transform(new Vector2(viewport.Width / Zoom, viewport.Height / Zoom), InverseTransform);
            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }
        #endregion
    }
}
