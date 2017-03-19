using System;
using ProjectPivot.Components;
using ProjectPivot.Entities;

namespace ProjectPivot {
    public class BulletPassthrough : Damageable {
        public bool TakeDamage(int damage, GameObject source) {
            return false;
        }
    }
}
