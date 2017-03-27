using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Components.AI {
    public class EnemyState : Damageable {
        protected Enemy enemy;
        public EnemyState(Enemy enemy) {
            this.enemy = enemy;
        }
        
        public virtual EnemyState Update(GameTime gameTime) {
            return this;
        }

        public virtual void Enter(EnemyState previousState) {
        }

        public virtual void Leave(EnemyState newState) {
        }

        public virtual bool TakeDamage(int damage, GameObject source) {
            return true;
        }
    }
}
