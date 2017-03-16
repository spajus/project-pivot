using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public class GameObject {

        public GameObject Parent { get; protected set; }
        public List<GameObject> Children { get; protected set; }


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


        public void Update(GameTime gameTime) {
            OnUpdate(gameTime);
			components.ForEach(component => component.Update(gameTime));
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

		protected virtual void OnInitialize() { }
		protected virtual void OnUpdate(GameTime gameTime) { }
		protected virtual void OnDraw(SpriteBatch spriteBatch) { }
    }
}
