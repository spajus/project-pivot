using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace ProjectPivot.Entities.Items {
    public static class ItemFactory {
        private static Dictionary<string, Item> itemPrototypes;

        public static void Initialize() {
            using (StreamReader file = File.OpenText(@"Data/Items.json")) {
                JsonSerializer serializer = new JsonSerializer();
                itemPrototypes = (Dictionary<string, Item>)serializer.Deserialize(
                    file, typeof(Dictionary<string, Item>));
            }
        }

        public static Item Build(string Id, Vector2 position) {
            Item item = JsonConvert.DeserializeObject<Item>(
                JsonConvert.SerializeObject(itemPrototypes[Id]));
            item.Position = position;
            return item;
        }
    }
}
