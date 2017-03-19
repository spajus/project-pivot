using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectPivot.Entities {

    public static class Weapons {
        private static Dictionary<string, Weapon> weaponPrototypes;

        public static void Initialize() {
            using (StreamReader file = File.OpenText(@"Data/Weapons.json")) {
                JsonSerializer serializer = new JsonSerializer();
                weaponPrototypes = (Dictionary<string, Weapon>)serializer.Deserialize(
                    file, typeof(Dictionary<string, Weapon>));
            }
        }

        public static Weapon Build(string name) {
            Console.WriteLine(JsonConvert.SerializeObject(new Weapon(new Vector2(10, 100))));
            Weapon weap = JsonConvert.DeserializeObject<Weapon>(
                JsonConvert.SerializeObject(weaponPrototypes[name]));
            return weap;
        }
    }
}
