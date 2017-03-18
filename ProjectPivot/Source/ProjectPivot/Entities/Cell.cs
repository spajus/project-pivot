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
    public class Cell : GameObject {

        public int MapX { get; protected set; }
        public int MapY { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Rectangle Area { get; protected set; }

        private Health health;
        public bool IsHealthy { get { return health.IsHealthy; } }

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
            if (ProjectPivot.cellsDebugEnabled) {
                Rectangle xhrect = new Rectangle(
                    Camera.Main.Crosshair.WorldPosition.ToPoint(), new Point(1, 1));
                if (Area.Intersects(xhrect)) {
                    Gizmo.Rectangle(Area, Color.Red);
                    Gizmo.Rectangle(xhrect, Color.Blue);
                    health.DrawHealth();
                }
            }
        }
    }
}
