using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public static class GameObjects {

        // CONSTANTS
        private const bool useQuadTree = true;

        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> alwaysUpdated = new List<GameObject>();
        private static Queue<GameObject> pendingDestruction = new Queue<GameObject>();
        private static QuadTree gameObjectTree;

        public static void Add(GameObject gameObject, bool alwaysUpdate = false) {
            gameObjects.Add(gameObject);
            if (useQuadTree) {
                if (alwaysUpdate) {
                    alwaysUpdated.Add(gameObject);
                }  else { 
                    gameObjectTree.Insert(gameObject);
                }
            }
            gameObject.Initialize();
        }

        public static void Destroy(GameObject gameObject) {
            pendingDestruction.Enqueue(gameObject);
        }

        public static GameObject Remove(GameObject gameObject) {
            gameObjects.Remove(gameObject);
            if (useQuadTree) {
                if (alwaysUpdated.Contains(gameObject)) {
                    alwaysUpdated.Remove(gameObject);
                }  else {
                    gameObjectTree.Remove(gameObject);
                }
            }
            return gameObject;
        }

        public static List<T> Nearby<T>(Vector2 position, float maxDistance) {
            float hx = position.X + maxDistance;
            float hy = position.Y + maxDistance;
            float maxDistanceSquared = maxDistance * maxDistance;
            List<GameObject> results = gameObjectTree.QueryRange(new AABB(position.X, position.Y, hx, hy));
            results.AddRange(alwaysUpdated);
            return results.Where(
                result => (result is T) && (Vector2.DistanceSquared(position, result.Position) 
                    <= maxDistanceSquared)).Cast<T>().ToList();
        }

        public static void Initialize(Map map) {
            gameObjects.Clear();
            alwaysUpdated.Clear();
            pendingDestruction.Clear();
            if (useQuadTree) {
                gameObjectTree = new QuadTree(map.Boundary);
            }
        }

        public static void UpdatePosition(GameObject gameObject) {
            if (useQuadTree) {
                if (!alwaysUpdated.Contains(gameObject)) {
                    gameObjectTree.Remove(gameObject);
                    gameObjectTree.Insert(gameObject);
                }
            }
        }

        public static void Update(GameTime gameTime) {
            while (pendingDestruction.Count > 0) {
                Remove(pendingDestruction.Dequeue()).AfterDestroy();
            }
            uint updates = 0;
            if (useQuadTree) {
                for (int i = 0; i < alwaysUpdated.Count; i++) {
                    alwaysUpdated[i].Update(gameTime);
                    updates++;
                }
                foreach (GameObject visibleObject in gameObjectTree.QueryRange(Camera.Main.VisibleAreaAABB)) {
                    visibleObject.Update(gameTime);
                    updates++;
                }
            } else { 
                gameObjects.ForEach(gameObject => gameObject.Update(gameTime));
                updates++;
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {
            List<GameObject> visibleObjects;
            if (useQuadTree) {
                foreach (GameObject go in alwaysUpdated) {
                    go.Draw(spriteBatch);
                }
                visibleObjects = gameObjectTree.QueryRange(Camera.Main.VisibleAreaAABB);
                foreach (GameObject visibleObject in visibleObjects) {
                    if (visibleObject.IsVisible(Camera.Main)) {
                        visibleObject.Draw(spriteBatch);
                    }
                }

            } else {
                visibleObjects = gameObjects.Where(gameObject => gameObject.IsVisible(Camera.Main)).ToList();
                foreach (GameObject visibleObject in visibleObjects) {
                    visibleObject.Draw(spriteBatch);
                }
            }
            if (true) {
                Gizmo.Text(
                    $"Visible objects: {visibleObjects.Count}",
                    Camera.Main.ToWorldCoordinates(new Vector2(0, 40)), Color.White);
                Gizmo.Text(
                    $"All objects: {gameObjects.Count}",
                    Camera.Main.ToWorldCoordinates(new Vector2(0, 58)), Color.White);
            }
        }
    }
}
