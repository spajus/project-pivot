using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Components;

namespace ProjectPivot.Entities {
    public class BloodSplat : GameObject {
        public BloodSplat(Vector2 position) : base(position) {
            PawnBlood component = AddComponent<PawnBlood>(new PawnBlood());
            component.LayerDepth = 0.95f; // just above ground
        }
    }
}
