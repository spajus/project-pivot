using System;
using System.Collections.Generic;
using ProjectPivot.Entities;
using ProjectPivot.Entities.Items;

namespace ProjectPivot.Components.Items {
    public class Inventory : Component {
        protected List<Item> items;
        public Inventory() {
            items = new List<Item>();

        }

        public void Pickup(Item item) {
            GameObjects.Remove(item); // no longer on the ground
            items.Add(item);
        }
    }
}
