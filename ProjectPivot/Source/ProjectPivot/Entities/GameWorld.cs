using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public class GameWorld {
        public static GameWorld Current;
        public int MapWidth = 60;
        public int MapHeight = 30;
        public World World;
        public Map Map;
        public static bool Initialized = false;

        public void Initialize() {
            FarseerPhysics.Settings.ContinuousPhysics = true;
            FarseerPhysics.Settings.AllowSleep = true;

            World = new World(Vector2.Zero);
            Map = Map.Current = new Map(MapWidth, MapHeight, Vector2.Zero);
            Map.World = World;

            GameObjects.Initialize(Map);

            Map.Generate();

            Player.Current = new Player(Map.RandomHollowCell().Position);
            GameObjects.Add(Player.Current, true);

            Camera.Main = new Camera(ProjectPivot.Current.GraphicsDevice.Viewport, Player.Current.Position);
            Camera.Main.Target = Player.Current;
            GameObjects.Add(Camera.Main, true);

            Weapons.Initialize();
            Player.Current.TakeWeapon(Weapons.Build("sniper_rifle"));

            GameObjects.Add(Player.Current.Weapon, true);

            Initialized = true;
        }

        public GameWorld() {
        }

        public void BuildLevel() {

        }
    }
}
