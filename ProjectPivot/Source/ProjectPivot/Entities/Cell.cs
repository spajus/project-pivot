using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;
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
            this.Area = new Rectangle((int) Position.X, (int) Position.Y, width, height);
            AddComponent(new CellGraphics());
            this.health = AddComponent<Health>(new Health(health));
            AddComponent(new CellBody());
        }

        public override bool IsVisible(Camera camera) {
            return camera.IsVisible(Area);
        }

        public virtual void OnUpdate(GameTime gameTime) {
            if (ProjectPivot.cellsDebugEnabled) {
                health.DrawHealth();
            }
        }
    }
}
