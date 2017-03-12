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
        public float Rotation;
        private Viewport viewport;
        private MouseState mouseState;
        private KeyboardState keyboardState;
        private Int32 prevMouseScrollValue;
		private float cameraSpeed = 500f;
		private float zoomSpeed = 30f;
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
            return Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), InverseTransform);
        }

        public bool IsVisible(Vector2 position) {
            // TODO
            return true;
        }

        public void Update(GameTime gameTime) {
            ReactToUserInput(gameTime);
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
            keyboardState = Keyboard.GetState();
            if (mouseState.ScrollWheelValue > prevMouseScrollValue) {
                Zoom += zoomSpeed * deltaTime;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
            } else if (mouseState.ScrollWheelValue < prevMouseScrollValue) {
                Zoom -= zoomSpeed * deltaTime;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
            }
            if (keyboardState.IsKeyDown(Keys.A)) {
				Position.X += cameraSpeed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.D)) {
				Position.X -= cameraSpeed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.W)) {
				Position.Y += cameraSpeed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.S)) {
				Position.Y -= cameraSpeed * deltaTime;
            }
        }
        #endregion
    }
}
