using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot {
    public static class Settings {
        public static Color BACKGROUND_COLOR = Color.White;

        public static bool PHYSICS_DEBUG = false;

        public static int MAP_WIDTH = 200;
        public static int MAP_HEIGHT = 200;

        public static bool DEBUG_GRID = false;
        public static bool DEBUG_MAP_BOUNDS = false;
        public static bool DEBUG_CELL_HEALTH = false;
        public static bool DEBUG_PAWN_BODY = false;
        public static bool DEBUG_RAYCAST = false;

        public static double MIN_PHYSICS_STEP_TIME = 1.0 / 30.0;
        public static SamplerState SAMPLER_STATE = SamplerState.PointWrap;
		public static Effect GLOBAL_SHADER = null;
		public static bool ENABLE_AUDIO = true;
    }
}
