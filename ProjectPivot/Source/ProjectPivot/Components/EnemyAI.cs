using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;

namespace ProjectPivot.Components {
    public class EnemyAI : Component {
        EnemyInput input;
        AiMotion motion;

        Cell targetCell;
        public EnemyAI() {
        }

        public override void Initialize() {
            input = GameObject.GetComponent<EnemyInput>();
        }

        public override void Update(GameTime gameTime) {
            if (targetCell == null) {
            }

        }
    }
}
