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

        public static void Remove(GameObject gameObject) {
            gameObjects.Remove(gameObject);
            if (useQuadTree) {
                if (alwaysUpdated.Contains(gameObject)) {
                    alwaysUpdated.Remove(gameObject);
                }  else {
                    gameObjectTree.Remove(gameObject);
                }
            }
            // TODO: gameObject.Destroy();
        }

        public static void Initialize(Map map) {
            if (useQuadTree) {
                gameObjectTree = new QuadTree(map.Boundary);
            }
        }

        public static void UpdatePosition(GameObject gameObject) {
            if (useQuadTree) {
                gameObjectTree.Remove(gameObject);
                gameObjectTree.Insert(gameObject);
            }
        }

        public static void Update(GameTime gameTime) {
            if (useQuadTree) {
                foreach (GameObject go in alwaysUpdated) {
                    go.Update(gameTime);
                }
                foreach (GameObject visibleObject in gameObjectTree.QueryRange(Camera.Main.VisibleAreaAABB)) {
                    visibleObject.Update(gameTime);
                }
            } else { 
                gameObjects.ForEach(gameObject => gameObject.Update(gameTime));
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {
            List<GameObject> visibleObjects;
            if (useQuadTree) {
                foreach (GameObject go in alwaysUpdated) {
                    go.Draw(spriteBatch);
                }
                Gizmo.Text($"Rekt: #{Camera.Main.VisibleAreaAABB}", 
                    Camera.Main.ToWorldCoordinates(new Vector2(0, 140)), Color.White);
                Gizmo.Text($"Map Rekt: #{gameObjectTree}", 
                    Camera.Main.ToWorldCoordinates(new Vector2(0, 160)), Color.White);
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
                    Camera.Main.ToWorldCoordinates(new Vector2(0, 100)), Color.White);
                Gizmo.Text(
                    $"All objects: {gameObjects.Count}",
                    Camera.Main.ToWorldCoordinates(new Vector2(0, 120)), Color.White);
            }
        }
    }
}
