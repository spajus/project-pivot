using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPivot.Entities;
using ProjectPivot.Utils;
using System.Diagnostics;

namespace ProjectPivot.Entities {
    public class Map {
        // in tiles
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        private Cell[,] cells;
        public List<Cell> HollowCells = new List<Cell>();
        private Random rand;

        public Map(int width, int height) {
            this.Width = width;
            this.Height = height;
            this.cells = new Cell[width, height];
            this.rand = new Random();
        }

        public void Generate() {
            Random rand = new Random();
            SimplexNoise.Seed = (int) rand.Next() * 10000000;
            float[,] noise = SimplexNoise.Calc2D(Width, Height, 0.04f);
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    int health = (int)((noise[x, y] + 10) / 255 * 100);
                    cells[x, y] = new Cell(x, y, 32, 32, health);
                    GameObjects.Add(cells[x, y]);
                    if (!cells[x, y].IsHealthy) {
                        HollowCells.Add(cells[x, y]);
                    }
                }
            }
            if (HollowCells.Count == 0) {
                throw new Exception("Could not generate map, no hollow cells!");
            }
        }

        public void Draw(Camera camera, SpriteBatch spriteBatch) {
            foreach (Cell cell in cells) {
                if (camera.IsVisible(cell.Area)) {
                    cell.Draw(spriteBatch);
                }
            }
        }

        public Cell RandomHollowCell() {
            int r = rand.Next(HollowCells.Count);
            return HollowCells[r];
        }
    }
}
