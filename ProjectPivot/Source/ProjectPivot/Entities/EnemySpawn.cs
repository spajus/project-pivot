using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    public static class EnemySpawn {
        static float nextSpawnCooldownMs = 1000f;

        public static void Update(GameTime gameTime) {
            nextSpawnCooldownMs -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnCooldownMs <= 0f) {
                Enemy enemy = new Enemy(GameWorld.Current.Map.RandomHollowCell().Position);
                Weapon wp = Weapons.Build("sniper_rifle");
                enemy.TakeWeapon(wp);
                GameObjects.Add(enemy, true);
                GameObjects.Add(wp, true);
                nextSpawnCooldownMs = 10000f;
            }
        }
    }
}
