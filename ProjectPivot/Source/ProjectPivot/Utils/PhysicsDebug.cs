using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Utils {
    class PhysicsDebug {
        World world;
        DebugViewXNA debugView;
        GraphicsDevice graphics;

        public PhysicsDebug(World world) {
            this.world = world;
            debugView = new DebugViewXNA(world);
            debugView.AppendFlags(DebugViewFlags.DebugPanel);
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content) {

            debugView.LoadContent(graphics, content);
            this.graphics = graphics;
        }

        public void Draw() {
            Matrix projection = Camera.Main.Transform;
            projection = Matrix.CreateOrthographicOffCenter(
                0f, ConvertUnits.ToSimUnits(graphics.Viewport.Width),
                ConvertUnits.ToSimUnits(graphics.Viewport.Height), 0f, 0f, 1f);

            Matrix view = 
				Matrix.CreateTranslation(ConvertUnits.ToSimUnits(
                    new Vector3(-Camera.Main.Position.X, -Camera.Main.Position.Y, 0))) *
				 Matrix.CreateScale(new Vector3(Camera.Main.Zoom, Camera.Main.Zoom, 1)) *
				      Matrix.CreateTranslation(ConvertUnits.ToSimUnits(graphics.Viewport.Width) * 0.5f, 
                      ConvertUnits.ToSimUnits(graphics.Viewport.Height) * 0.5f, 0);
            debugView.DebugPanelPosition = -Vector2.One;
            debugView.RenderDebugData(ref projection, ref view);
        }
    }
}
