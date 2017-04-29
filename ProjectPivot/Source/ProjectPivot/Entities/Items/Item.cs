using System;
using ProjectPivot.Components.Items;

namespace ProjectPivot.Entities.Items {
    public class Item : GameObject {
        public string Id;
        public string Name;

        protected override void OnInitialize() {
            AddComponent(new ItemGraphics(Id));
        }
    }
}
