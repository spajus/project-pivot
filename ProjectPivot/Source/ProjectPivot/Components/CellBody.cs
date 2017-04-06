using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using ProjectPivot.Pathfinding;
using System.Collections.Generic;
using ProjectPivot.Utils;

namespace ProjectPivot.Components {
    public class CellBody : Component {
        public Body Body { get; protected set; }
        private Health health;

        public override void Initialize() {
            this.health = GameObject.GetComponent<Health>();
            AddBodyIfNecessary();
        }

        public void AddBodyIfNecessary() {
            if (this.Body != null) {
                return;
            }
            if (!health.IsHealthy) {
                return;
            }

            if (Map.Current.HasUnhealthyNeighbours((Cell)GameObject)) {
                this.Body = BodyFactory.CreateRectangle(
                    GameWorld.Current.World,
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
                Body = null;
                Map.Current.HollowCells.Add((Cell)GameObject);
                foreach (Cell c in ((Cell) GameObject).Neighbours(true)) {
                    c.GetComponent<CellBody>().AddBodyIfNecessary();
                }
            }
        }
    }
}
