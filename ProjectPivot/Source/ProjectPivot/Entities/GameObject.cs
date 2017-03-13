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
        List<Component> components = new List<Component>();

        public Vector2 Position { get; protected set; }

        #region Constructor

        public GameObject(Vector2 position) {
            this.Position = position;
        }
        public GameObject(Vector2 position, GameObject parent = null) {
            this.Position = position;
            this.Parent = parent;
        }

        #endregion

        public void AddComponent(Component component) {
            this.components.Add(component);
            component.GameObject = this;
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

        public void Update(GameTime gameTime) {
            components.ForEach(component => component.Update(gameTime));
        }

        public void Draw(SpriteBatch spriteBatch) {
            components.ForEach(component => component.Draw(spriteBatch));
        }
    }
}
