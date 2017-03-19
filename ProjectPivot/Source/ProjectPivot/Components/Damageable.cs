using System;
using ProjectPivot.Entities;

namespace ProjectPivot.Components {
    public interface Damageable {
        bool TakeDamage(int damage, GameObject source);
    }
}
