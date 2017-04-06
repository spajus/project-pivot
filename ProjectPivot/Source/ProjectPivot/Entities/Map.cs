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
using SharpNoise.Modules;
using ProjectPivot.Components;

namespace ProjectPivot.Entities {
    public class Map {
        public static Map Current;
        // in tiles
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public AABB Boundary { get; protected set; }
        private Vector2 offset;
        private Dictionary<Point, Cell> cells;
        public List<Cell> HollowCells = new List<Cell>();

        internal List<Cell> CellsAroundWorldPoint(Vector2 position, int radius = 15) {
            int cx = (int) (position.X + 16) / 32;
            int cy = (int) (position.Y + 16) / 32;
            List<Cell> result = new List<Cell>();
            for (int x = cx - radius; x <= cx + radius; x++) {
                for (int y = cy - radius; y <= cy + radius; y++) {
                    result.Add(CellAt(x, y));
                }
            }
            return result;
        }

        private Random rand;
        public World World;

        BulletPassthrough wall = new BulletPassthrough();

        Perlin noise;


        public Map(int width, int height, Vector2 offset) {
            Width = width;
            Height = height;
            cells = new Dictionary<Point, Cell>();
            rand = new Random();
            this.offset = offset;
            Rectangle mapBounds = new Rectangle((int)offset.X - 16, (int) offset.Y - 16, width * 32, height * 32);
            Boundary = new AABB(mapBounds, Color.Brown);
            if (Settings.DEBUG_MAP_BOUNDS) {
                Gizmo.Rectangle(mapBounds, Color.Violet, true);
                Gizmo.Rectangle(Boundary.ToRectangle(), Color.Brown, true);
                //Gizmo.Text("x", Boundary.Center, Color.Brown, true);
            }
            noise = new Perlin {
                Frequency = 0.073,
                Persistence = 0.5,
                Lacunarity = 1,
                OctaveCount = 1,
                Quality = SharpNoise.NoiseQuality.Standard
            };
        }

        public float CellHealthAt(int x, int y) {
            Point p = new Point(x, y);
            if (cells.ContainsKey(p)) {
                return cells[p].Health;
            }
            return CellHealthFromNoise(x, y);
            
        }

        float CellHealthFromNoise(int x, int y) {
            return (float) ((noise.GetValue(x, y, 0) + 1) * 50);
        }

        public bool HasUnhealthyNeighbours(Cell cell) {
            for (int x = cell.MapX - 1; x <= cell.MapX + 1; x += 1) {
                for (int y = cell.MapY - 1; y <= cell.MapY + 1; y +=1) {
                    if (!Health.IsGameObjectHealthy(CellHealthAt(x, y))) {
                        return true;
                    }

                }
            }
            return false;
        }

        public Cell CellAtWorld(Vector2 position) {
            int x = 0;
            int y = 0;
            if (position.X >= 0) {
                x = (int) (position.X + 16) / 32;
            } else {
                x = (int) (position.X - 16) / 32;

            }
            if (position.Y >= 0) {
                y = (int) (position.Y + 16) / 32;
            } else {
                y = (int) (position.Y - 16) / 32;

            }
            return CellAt(x, y);
        }

        public Cell CellAt(int x, int y) {
            Point p = new Point(x, y);
            if (cells.ContainsKey(p)) {
                return cells[p];
            } else {
                Cell c = GenerateCellAt(x, y);
                return c; 
            }
        }

        // http://math.stackexchange.com/questions/76457/check-if-a-point-is-within-an-ellipse
        bool pointInMapEllipse(int x, int y) {
            float centerX = Width / 2 + offset.X;
            float centerY = Height / 2 + offset.Y;  
            return Math.Pow(x - centerX, 2) / Math.Pow(Width / 2, 2) 
                + Math.Pow(y - centerY, 2) / Math.Pow(Height / 2, 2) <= 1;
        }

        public void Update(GameTime gameTime) {
            Rectangle visible = Camera.Main.VisibleArea;
            for (int x = (visible.X / 32) - 1 ; x <= ((visible.X + visible.Width) / 32) + 1; x++) {
                for (int y = (visible.Y / 32) - 1; y <= ((visible.Y + visible.Height) / 32) + 1; y++) {
                    Cell c = CellAt(x, y);
                    if (c == null) {
                        c = GenerateCellAt(x, y);
                    }
                    if (c != null) {
                        c.Update(gameTime);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Rectangle visible = Camera.Main.VisibleArea;
            for (int x = (visible.X / 32) - 1; x <= ((visible.X + visible.Width) / 32) + 1; x++) {
                for (int y = (visible.Y / 32) - 1; y <= ((visible.Y + visible.Height) / 32) + 1; y++) {
                    Cell c = CellAt(x, y);
                    if (c != null) {
                        c.Draw(spriteBatch);
                    }
                }
            }
        }

        public Cell GenerateCellAt(int x, int y) {
            int health = (int)CellHealthFromNoise(x, y);
            Cell c = cells[new Point(x, y)] = new Cell(x, y, 32, 32, health, offset);
            c.Initialize();
            if (!c.IsHealthy) {
                HollowCells.Add(c);
            }
            return c;
        }

        public void Generate() {
            Random rand = new Random();
            SimplexNoise.Seed = (int) rand.Next() * 10000000;
            Vector2 center = new Vector2(Width / 2, Height / 2);

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    GenerateCellAt(x, y);
                }
            }
            if (HollowCells.Count == 0) {
                throw new Exception("Could not generate map, no hollow cells!");
            }
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
            topBound.UserData = wall;
            topBound.BodyType = BodyType.Static;
            bottomBound.UserData = wall;
            bottomBound.BodyType = BodyType.Static;
            leftBound.UserData = wall;
            leftBound.BodyType = BodyType.Static;
            rightBound.UserData = wall;
            rightBound.BodyType = BodyType.Static;
        }

        public Cell RandomHollowCell() {
            int r = rand.Next(HollowCells.Count);
            return HollowCells[r];
        }
    }
}
