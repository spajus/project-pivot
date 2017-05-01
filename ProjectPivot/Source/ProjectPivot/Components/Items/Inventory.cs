using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPivot.Entities;
using ProjectPivot.Entities.Items;

namespace ProjectPivot.Components.Items {
    public class Inventory : Component {
        protected Dictionary<string, ItemStack> items;
        public Inventory() {
            items = new Dictionary<string, ItemStack>();
        }

        public void Pickup(Item item) {
            GameObjects.Remove(item);
            if (items.ContainsKey(item.Id)) {
                items[item.Id].Add(item);
            } else {
                items.Add(item.Id, new ItemStack(item));
            }
            Console.WriteLine(this);
        }

        public override string ToString() {
            return string.Format("[Inventory items={0}]", 
               string.Join(", ", items.Select(kvp => kvp.Key + ": " + kvp.Value.ToString())));
        }
    }
}
