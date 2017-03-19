using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using ProjectPivot.Utils;
using FarseerPhysics.Dynamics;

namespace ProjectPivot.Entities {
    public class GameObject {

        public GameObject Parent { get; protected set; }
        public List<GameObject> Children { get; protected set; }
        private AABB box;

        // A bit bigger than actual object to prevent cutting
        public AABB QuadTreeBox {
            get {
                if (box == null) {
                    box = new AABB(
                        new Rectangle((int)Position.X, 
                                      (int)Position.Y, 
                                      32, 
                                      32));
                }
                return box;
            }
            protected set {
                box = value;
            }
        }

        public Vector2 Position;
        List<Component> components = new List<Component>();

        #region Constructor

        public GameObject(Vector2 position) {
            this.Position = position;
        }
        public GameObject(Vector2 position, GameObject parent = null) {
            this.Position = position;
            this.Parent = parent;
        }

        #endregion

        public object AddComponent(Component component) {
            return this.AddComponent<object>(component);
        }

        public T AddComponent<T>(Component component) {
            this.components.Add(component);
            component.GameObject = this;
            return (T) ((object) component);
        }

        public int X {
            get { return (int) Position.X; }
        }

        public int Y {
            get { return (int) Position.Y; }
        }


        public void AddChild(GameObject child) {
            Children.Add(child);
            child.Parent = this;
        }

        public void Move(Vector2 newPosition) {
            Position = newPosition;
        }

        public void Initialize() {
            OnInitialize();
            components.ForEach(component => component.Initialize());
        }

        public void Reinitialize() {
            OnInitialize();
            components.ForEach(component => component.Initialize());
        }


        public void Update(GameTime gameTime) {
            OnUpdate(gameTime);
			components.ForEach(component => component.Update(gameTime));
        }

        public void Destroy() {
            OnDestroy();
        }

        public void Draw(SpriteBatch spriteBatch) {
			OnDraw(spriteBatch);
            components.ForEach(component => component.Draw(spriteBatch));
        }

        public T GetComponent<T>() {
            return (T) (components.FirstOrDefault(t => t is T) as object);
        }

        public virtual bool IsVisible(Camera camera) {
            return true;
        }

        public virtual Body PhysicsBody() { return null; }

		protected virtual void OnInitialize() { }
		protected virtual void OnUpdate(GameTime gameTime) { }
		protected virtual void OnDraw(SpriteBatch spriteBatch) { }
        protected virtual void OnDestroy() { }
    }
}
