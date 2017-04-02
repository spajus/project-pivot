using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using ProjectPivot.Pathfinding;

namespace ProjectPivot.Components {
    public class CellBody : Component {
        public Body Body { get; protected set; }
        private Health health;

        public override void Initialize() {
            this.health = GameObject.GetComponent<Health>();
            if (health.IsHealthy) {
                this.Body = BodyFactory.CreateRectangle(
                    Map.Current.World,
                    ConvertUnits.ToSimUnits(32),
                    ConvertUnits.ToSimUnits(32),
                    1.0f);
                Body.Mass = 1f;
                Body.Restitution = 0.02f;
                Body.BodyType = BodyType.Static;
                Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
                Body.UserData = GameObject;
           }
        }

        public override void Update(GameTime gameTime) {
            if (!health.IsHealthy && Body != null) {
                Map.Current.World.RemoveBody(Body);
                CellGraph.Current.RegenerateGraphAtCell((Cell) GameObject);
                Body = null;
                Map.Current.HollowCells.Add((Cell)GameObject);
            }
        }
    }
}
