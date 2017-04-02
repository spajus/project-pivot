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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics;
using ProjectPivot.Pathfinding;

namespace ProjectPivot.Entities {
    public class Map {
        public static Map Current;
        // in tiles
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public AABB Boundary { get; protected set; }
        private Vector2 offset;
        private Cell[,] cells;
        public List<Cell> HollowCells = new List<Cell>();
        private Random rand;
        public World World;


        public Map(int width, int height, Vector2 offset) {
            Width = width;
            Height = height;
            cells = new Cell[width, height];
            rand = new Random();
            this.offset = offset;
            Rectangle mapBounds = new Rectangle((int)offset.X - 16, (int) offset.Y - 16, width * 32, height * 32);
            Boundary = new AABB(mapBounds, Color.Brown);
            if (ProjectPivotOld.mapDebugEnabled) {
                Gizmo.Rectangle(mapBounds, Color.Violet, true);
                Gizmo.Rectangle(Boundary.ToRectangle(), Color.Brown, true);
                //Gizmo.Text("x", Boundary.Center, Color.Brown, true);
            }
        }

        public Cell CellAtWorld(Vector2 position) {
            int x = (int) (position.X + 16) / 32;
            int y = (int) (position.Y + 16) / 32;
            return CellAt(x, y);
        }

        public Cell CellAt(int x, int y) {
            if (x >= Width || x < 0 || y >= Height || y < 0) {
                return null;
            }
            return cells[x, y];
        }

        public void Generate() {
            Random rand = new Random();
            SimplexNoise.Seed = (int) rand.Next() * 10000000;
            float[,] noise = SimplexNoise.Calc2D(Width, Height, 0.04f);
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    int health = (int)((noise[x, y] - 10) / 255 * 100);
                    cells[x, y] = new Cell(x, y, 32, 32, health, offset);
                    GameObjects.Add(cells[x, y]);
                    if (!cells[x, y].IsHealthy) {
                        HollowCells.Add(cells[x, y]);
                    }
                }
            }
            if (HollowCells.Count == 0) {
                throw new Exception("Could not generate map, no hollow cells!");
            }
            CreatePhysicsBounds();
            CalculatePathfinding();
        }

        public void CalculatePathfinding() {
            CellGraph.Current = new CellGraph(this);
        }

        public void CreatePhysicsBounds() {
            Rectangle worldRect = Boundary.ToRectangle();
            Body topBound = BodyFactory.CreateRectangle(
                World,
                ConvertUnits.ToSimUnits(worldRect.Width),
                ConvertUnits.ToSimUnits(16),
                1f,
                ConvertUnits.ToSimUnits(
                    new Vector2(Boundary.Center.X,
                                Boundary.Center.Y - worldRect.Height / 2 - 8)));
            Body bottomBound = BodyFactory.CreateRectangle(
                World,
                ConvertUnits.ToSimUnits(worldRect.Width),
                ConvertUnits.ToSimUnits(16),
                1f,
                ConvertUnits.ToSimUnits(
                    new Vector2(Boundary.Center.X,
                                Boundary.Center.Y + worldRect.Height / 2 + 8)));

            Body leftBound = BodyFactory.CreateRectangle(
                World,
                ConvertUnits.ToSimUnits(16),
                ConvertUnits.ToSimUnits(worldRect.Height + 32),
                1f,
                ConvertUnits.ToSimUnits(
                    new Vector2(Boundary.Center.X - worldRect.Width / 2 - 8,
                                Boundary.Center.Y)));
            Body rightBound = BodyFactory.CreateRectangle(
                World,
                ConvertUnits.ToSimUnits(16),
                ConvertUnits.ToSimUnits(worldRect.Height + 32),
                1f,
                ConvertUnits.ToSimUnits(
                    new Vector2(Boundary.Center.X + worldRect.Width / 2 + 8,
                                Boundary.Center.Y)));
            BulletPassthrough wall = new BulletPassthrough();
            topBound.UserData = wall;
            topBound.BodyType = BodyType.Static;
            bottomBound.UserData = wall;
            bottomBound.BodyType = BodyType.Static;
            leftBound.UserData = wall;
            leftBound.BodyType = BodyType.Static;
            rightBound.UserData = wall;
            rightBound.BodyType = BodyType.Static;
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
