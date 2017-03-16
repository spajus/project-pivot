using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectPivot.Entities;
using ProjectPivot.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
	public class Camera : GameObject {

        public static Camera Main;

        #region Fields
        public float Zoom;
        public Matrix Transform;
        public Matrix InverseTransform { get; protected set; }
        public Vector2 WorldPosition { get; protected set; }
        public Rectangle VisibleArea { get; protected set; }
        public AABB VisibleAreaAABB { get {
                return new AABB(VisibleArea, Color.YellowGreen);}
        }
        public float Rotation;
        private Viewport viewport;
        private MouseState mouseState;
        private Int32 prevMouseScrollValue;
		private float cameraSpeed = 4f;
		private float zoomSpeed = 3f;
        public GameObject Target;
        #endregion

        #region Constructor
		public Camera(Viewport viewport, Vector2 position) : base(position) {
            this.Zoom = 1.0f;
            this.Rotation = 0.0f;
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
            return VisibleArea.Intersects(position);
        }

        public Vector2 ToWorldCoordinates(Vector2 screenPosition) {
            return Vector2.Transform(screenPosition, InverseTransform);
        }

        #endregion

        #region Protected Functions
        protected override void OnUpdate(GameTime gameTime)
		{
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			ReactToUserInput(deltaTime);
			Zoom = MathHelper.Clamp(Zoom, 0.5f, 1.5f);
			LerpToTarget(deltaTime);
			WorldPosition = ToWorldCoordinates(Position);
			VisibleArea = CalculateVisibleArea();
            Gizmo.Rectangle(VisibleArea, Color.Pink);
			// Rotation = ClampRotation();

			Transform = 
				Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
				Matrix.CreateRotationZ(Rotation) *
				 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
				      Matrix.CreateTranslation(viewport.Width * 0.5f, viewport.Height * 0.5f, 0);

			InverseTransform = Matrix.Invert(Transform);

			Gizmo.Rectangle(VisibleArea, Color.Blue);
			Gizmo.Line(Position, Target.Position, Color.Red);
		}

		void ReactToUserInput(float deltaTime) {
            mouseState = Mouse.GetState();
            if (mouseState.ScrollWheelValue > prevMouseScrollValue) {
                Zoom += zoomSpeed * deltaTime;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
				Debug.WriteLine($"Zoom: {Zoom}");

            } else if (mouseState.ScrollWheelValue < prevMouseScrollValue) {
                Zoom -= zoomSpeed * deltaTime;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
				Debug.WriteLine($"Zoom: {Zoom}");
            }
        }

        void LerpToTarget(float deltaTime) {
			if (Target != null) {
				if (Vector2.DistanceSquared(Position, Target.Position) > 500) {
					Position = Vector2.Lerp(Position, Target.Position, deltaTime * cameraSpeed);
                }
            }
        }

        Rectangle CalculateVisibleArea() {
            var tl = Vector2.Transform(Vector2.Zero, InverseTransform);
            var tr = Vector2.Transform(new Vector2(viewport.Width, 0), InverseTransform);
            var bl = Vector2.Transform(new Vector2(0, viewport.Height), InverseTransform);
            var br = Vector2.Transform(new Vector2(viewport.Width, viewport.Height), InverseTransform);
            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));

            return new Rectangle(
                (int)min.X,
                (int)min.Y,
                (int)(max.X - min.X),
                (int)(max.Y - min.Y));
        }
        #endregion
    }
}
