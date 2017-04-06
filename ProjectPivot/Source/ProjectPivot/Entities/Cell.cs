using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;
using ProjectPivot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public class Cell : GameObject, Damageable {

        public int MapX { get; protected set; }
        public int MapY { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Rectangle Area { get; protected set; }

        private Health health;
        public bool IsHealthy { get { return health.IsHealthy; } }
        public float Health { get { return health.Value; } }

        public Cell(int mapX, int mapY, int width, int height, int health, Vector2 offset) : 
                base(position: new Vector2(mapX * width, mapY * height) + offset) {
            this.Width = width;
            this.Height = height;
            this.MapX = mapX;
            this.MapY = mapY;
            this.Area = new Rectangle((int) Position.X - 16, (int) Position.Y - 16, width, height);
            AddComponent(new CellGraphics());
            this.health = AddComponent<Health>(new Health(health));
            AddComponent(new CellBody());
        }

        public override bool IsVisible(Camera camera) {
            return camera.IsVisible(Area);
        }

        protected override void OnUpdate(GameTime gameTime) {
            if (ProjectPivotOld.cellsDebugEnabled) {
                Rectangle xhrect = new Rectangle(
                    Camera.Main.Crosshair.WorldPosition.ToPoint(), new Point(1, 1));
                if (Area.Intersects(xhrect)) {
                    Gizmo.Rectangle(Area, Color.Red);
                    Gizmo.Rectangle(xhrect, Color.Blue);
                    health.DrawHealth();
                }
            }
        }

        public float PathfindingCost {
            get {
                if (IsHealthy) {
                    return 0f;
                } else {
                    return 1f;
                }
            }
        }
        
        public bool IsClippingCorner(Cell neighbourCell) {
            // If the movement from curr to neigh is diagonal (e.g. N-E)
            // Then check to make sure we aren't clipping (e.g. N and E are both walkable)
            int dX = this.X - neighbourCell.X;
            int dY = this.Y - neighbourCell.Y;

            if (Math.Abs(dX) + Math.Abs(dY) == 2) {
                // We are diagonal
                if (Map.Current.CellAt(X - dX, Y).PathfindingCost < 0.01f) {
                    // East or West is unwalkable, therefore this would be a clipped movement.
                    return true;
                }

                if (Map.Current.CellAt(X, Y - dY).PathfindingCost < 0.01f) {
                    // North or South is unwalkable, therefore this would be a clipped movement.
                    return true;
                }

                // If we reach here, we are diagonal, but not clipping
            }

            // If we are here, we are either not clipping, or not diagonal
            return false;
        }

        public Cell[] Neighbours(bool diagonalOk = false) {
            Cell[] cells = new Cell[8];
            cells[0] = Map.Current.CellAt(MapX, MapY + 1);
            cells[1] = Map.Current.CellAt(MapX + 1, MapY);
            cells[2] = Map.Current.CellAt(MapX, MapY - 1);
            cells[3] = Map.Current.CellAt(MapX - 1, MapY);
            if (diagonalOk) {
                cells[4] = Map.Current.CellAt(MapX + 1, MapY + 1);
                cells[5] = Map.Current.CellAt(MapX + 1, MapY - 1);
                cells[6] = Map.Current.CellAt(MapX - 1, MapY + 1);
                cells[7] = Map.Current.CellAt(MapX - 1, MapY - 1);
            }
            return cells;
        }

        public bool IsNeighbour(Cell cell, bool diagonalOk = true) {
            return Neighbours(diagonalOk).Contains(cell);
        } 

        public bool TakeDamage(int damage, GameObject source) {
            bool wasHealthy = health.IsHealthy;
            health.Decrease(damage / 2);
            if (wasHealthy && !health.IsHealthy) {
                AddComponent(new CellDebris());
            }
            return true;
        }
    }
}
