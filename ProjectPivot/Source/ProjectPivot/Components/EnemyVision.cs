using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using ProjectPivot.Pathfinding;

namespace ProjectPivot.Components {
    public class EnemyVision : Component {
        Enemy enemy;
        float updateCooldownMs = 0f;
        public List<GameObject> VisibleObjects = new List<GameObject>();
        public List<Cell> VisibleCells = new List<Cell>();
        public override void Initialize() {
            enemy = (Enemy)GameObject;
        }
        public override void Update(GameTime gameTime) {
            if (updateCooldownMs <= 0f) {
                VisibleObjects = GameObjects.Nearby<GameObject>(enemy.Position, 15f * 32);
                updateCooldownMs = 500f;
                VisibleCells = Map.Current.CellsAroundWorldPoint(enemy.Position);
                if (enemy.CellGraph == null) {
                    enemy.CellGraph = new CellGraph(VisibleCells);
                }
            }
            updateCooldownMs -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public List<Cell> HollowCells() {
            return VisibleCells.FindAll(c => !c.IsHealthy).ToList();
        }

        public T FindVisible<T>() {
            return VisibleObjects.FindAll(c => c is T).Cast<T>().FirstOrDefault();
        }
    }
}
