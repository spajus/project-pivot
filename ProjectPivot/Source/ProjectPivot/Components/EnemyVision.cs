using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;

namespace ProjectPivot.Components {
    public class EnemyVision : Component {
        Enemy enemy;
        float updateCooldownMs = 0f;
        public List<GameObject> VisibleObjects = new List<GameObject>();
        public override void Initialize() {
            enemy = (Enemy)GameObject;
        }
        public override void Update(GameTime gameTime) {
            if (updateCooldownMs <= 0f) {
                VisibleObjects = GameObjects.Nearby<GameObject>(enemy.Position, 15f * 32);
                updateCooldownMs = 500f;
            }
            updateCooldownMs -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}