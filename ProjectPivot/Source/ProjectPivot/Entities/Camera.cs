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
using ProjectPivot.Components;

namespace ProjectPivot.Entities {
	public class Camera : GameObject {

        public static Camera Main;

        #region Fields
        private int zoom = 100;
        public float Zoom { get { return zoom / 100f; } }
        public Matrix Transform;
        public Matrix InverseTransform { get; protected set; }
        public Vector2 WorldPosition { get; protected set; }
        public Rectangle VisibleArea { get; protected set; }
        public AABB VisibleAreaAABB { get {
                return new AABB(EnlargeRect(VisibleArea, 32), Color.YellowGreen);}
        }
        public float Rotation;
        private Viewport viewport;
        private MouseState mouseState;
        private Int32 prevMouseScrollValue;
		private float cameraSpeed = 4f;
        private int zoomSpeed = 5;

        public GameObject Target;

        public Crosshair Crosshair;


        #endregion

        #region Constructor
		public Camera(Viewport viewport, Vector2 position) : base(position) {
            this.Rotation = 0.0f;
            this.viewport = viewport;
            this.Crosshair = AddComponent<Crosshair>(new Crosshair());
        }
        #endregion

        #region Public Methods
        public Rectangle EnlargeRect(Rectangle input, int amount) {
            return new Rectangle(
                input.X - amount, input.Y - amount, 
                input.Width + amount * 2, input.Height + amount * 2);
        }
        public Vector2 MouseWorldCoordinates() {
            return ToWorldCoordinates(new Vector2(mouseState.X, mouseState.Y));
        }

        public bool IsVisible(Vector2 position) {
            return VisibleArea.Contains(position);
        }

        public bool IsVisible(Rectangle position) {
            return VisibleArea.Intersects(position);
        }

        public Vector2 ToWorldCoordinates(int x, int y) {
            return ToWorldCoordinates(new Vector2(x, y));
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
			zoom = MathHelper.Clamp(zoom, 75, 150);
			LerpToTarget(deltaTime);
			WorldPosition = ToWorldCoordinates(Position);
			VisibleArea = CalculateVisibleArea();
            // Gizmo.Rectangle(VisibleArea, Color.Pink);
			// Rotation = ClampRotation();

			Transform = 
				Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
				Matrix.CreateRotationZ(Rotation) *
				 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
				      Matrix.CreateTranslation(viewport.Width * 0.5f, viewport.Height * 0.5f, 0);

			InverseTransform = Matrix.Invert(Transform);

			// Gizmo.Rectangle(VisibleArea, Color.Blue);
			// Gizmo.Line(Position, Target.Position, Color.Red);
		}

		void ReactToUserInput(float deltaTime) {
            mouseState = Mouse.GetState();
            if (mouseState.ScrollWheelValue > prevMouseScrollValue) {
                zoom += zoomSpeed;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
            } else if (mouseState.ScrollWheelValue < prevMouseScrollValue) {
                zoom -= zoomSpeed;
                prevMouseScrollValue = mouseState.ScrollWheelValue;
            }
        }

        void LerpToTarget(float deltaTime) {
            Vector2 wantPosition = Vector2.Lerp(Target.Position, Crosshair.WorldPosition, 0.3f);
			if (Target != null) {
				if (Vector2.DistanceSquared(Position, wantPosition) > 500) {
					Position = Vector2.Lerp(Position, wantPosition, deltaTime * cameraSpeed);
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
