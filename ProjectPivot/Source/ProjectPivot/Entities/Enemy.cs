using System;
using Microsoft.Xna.Framework;
using ProjectPivot.Components;

namespace ProjectPivot.Entities {
    public class Enemy : GameObject {
        public Enemy(Vector2 position) : base(position) {
            AddComponent(new PawnBody());
            AddComponent(new PawnGraphics());
            AddComponent(new EnemyInput());
        }
    }
}
