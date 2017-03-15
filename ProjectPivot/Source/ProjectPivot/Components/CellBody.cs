using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ProjectPivot.Components {
    public class CellBody : Component {
        public Body Body { get; protected set; }
        private Health health;

        public override void Initialize() {
            this.health = GameObject.GetComponent<Health>();
            if (health.Value >= 40) {
                this.Body = BodyFactory.CreateCircle(
                    ProjectPivot.World,
                    ConvertUnits.ToSimUnits(16),
                    //ConvertUnits.ToSimUnits(32),
                    1.0f);
                Body.Mass = 0.1f;
                Body.BodyType = BodyType.Static;
                Body.Position = ConvertUnits.ToSimUnits(GameObject.Position);
            }
        }
	}
}
