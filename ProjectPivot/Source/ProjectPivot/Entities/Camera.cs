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
        private float wantZoom = 100;
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
            this.Rotation = 0;
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

        public Rectangle ToWorldRectangle(Rectangle mapRect) {
            Vector2 screenPos = ToWorldCoordinates(mapRect.X, mapRect.Y);
            return new Rectangle((int) screenPos.X, (int) screenPos.Y, mapRect.Width, mapRect.Height);
        }
        #endregion

        #region Protected Functions
        protected override void OnUpdate(GameTime gameTime)
		{
            Vector2 previousPosition = Position;
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			ReactToUserInput(deltaTime);
            LerpToZoom(deltaTime);
			LerpToTarget(deltaTime);
			WorldPosition = ToWorldCoordinates(Position);
			VisibleArea = CalculateVisibleArea();
            // Gizmo.Rectangle(VisibleArea, Color.Pink);
            // Rotation = ClampRotation();
            //fixme
            //clampToMapBounds(previousPosition);

			Transform = 
                Matrix.CreateTranslation(new Vector3((int) -Position.X,
                                                     (int) -Position.Y, 0)) *
				Matrix.CreateRotationZ(Rotation) *
                      Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                      Matrix.CreateTranslation((int) (viewport.Width * 0.5f), 
                                               (int) (viewport.Height * 0.5f),
                                               0);

			InverseTransform = Matrix.Invert(Transform);

			// Gizmo.Rectangle(VisibleArea, Color.Blue);
			// Gizmo.Line(Position, Target.Position, Color.Red);
		}

        // fixme
        void clampToMapBounds(Vector2 previousPosition) {
            Rectangle mapBounds = Map.Current.Boundary.ToRectangle();
            Rectangle updatedVA = CalculateVisibleArea();
            if (updatedVA.Top < mapBounds.Top || updatedVA.Bottom > mapBounds.Bottom) {
                Position.Y = previousPosition.Y;
            }
            if (updatedVA.Left < mapBounds.Left || updatedVA.Right > mapBounds.Right) {
                Position.X = previousPosition.X;
            }
        }

        void LerpToZoom(float deltaTime) {
            // 75 - 150
            /*
            float distSquared = Vector2.Distance(Position, Crosshair.WorldPosition);
            float lerpAmount = (int) 1 / (distSquared / 100f);

            if (lerpAmount < 0.01f) {
                lerpAmount = 0.01f;
            } 
            if (lerpAmount > 1f) {
                lerpAmount = 1f;
            }
            wantZoom = MathHelper.Lerp(75f, 150f, lerpAmount);
            zoom = (int) MathHelper.Lerp(zoom, wantZoom, deltaTime);
            */
            zoom = (int) MathHelper.Clamp(zoom, 75f, 150f);

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
			if (Target != null) {
                Vector2 wantPosition = Vector2.Lerp(Target.Position, Crosshair.WorldPosition, 0.3f);
                float distSquared = Vector2.DistanceSquared(Position, wantPosition);
                if (distSquared > 500) {
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
