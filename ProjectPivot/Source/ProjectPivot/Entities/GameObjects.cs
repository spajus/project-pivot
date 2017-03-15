using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public static class GameObjects {
        private static List<GameObject> gameObjects = new List<GameObject>();

        public static void Add(GameObject gameObject) {
            gameObjects.Add(gameObject);
            gameObject.Initialize();
        }

        public static void Remove(GameObject gameObject) {
            gameObjects.Remove(gameObject);
            // TODO: gameObject.Destroy();
        }

        public static void Initialize() {
            gameObjects.ForEach(gameObject => gameObject.Initialize());
        }

        public static void Update(GameTime gameTime) {
            gameObjects.ForEach(gameObject => gameObject.Update(gameTime));
        }

        public static void Draw(SpriteBatch spriteBatch) {
            foreach (GameObject visibleObject in gameObjects.Where(gameObject => gameObject.IsVisible(Camera.Main))) {
                visibleObject.Draw(spriteBatch);
            }
        }
    }
}
