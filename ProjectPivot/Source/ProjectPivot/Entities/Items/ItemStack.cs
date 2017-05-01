using System;
namespace ProjectPivot.Entities.Items {
    public class ItemStack {
        public string Id { get { return item.Id; } }
        int count = 0;
        Item item;

        public ItemStack(Item item, int count = 1) {
            this.item = item;
            this.count = count;
        }

        public void Add(Item item) {
            count += 1;
        }

        public override string ToString() {
            return string.Format("[ItemStack: Id={0}, count={1}]", Id, count);
        }
    }
}
