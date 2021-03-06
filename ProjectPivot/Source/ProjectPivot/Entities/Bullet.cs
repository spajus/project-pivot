using Microsoft.Xna.Framework;
using ProjectPivot.Components;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Entities {
    class Bullet : GameObject {
        private BulletPhysics physics;
        public GameObject Shooter;
        public Bullet(GameObject shooter, Vector2 origin, Vector2 target) : base(origin) {
            Shooter = shooter;
            physics = AddComponent<BulletPhysics>(new BulletPhysics(shooter, target));
            AddComponent(new BulletGraphics());
        }

        protected override void OnDestroy() {
            Map.Current.World.RemoveBody(physics.Body);
        }
    }
}
