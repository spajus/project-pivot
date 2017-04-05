using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Utils {
    public static class Sounds {
        public static Dictionary<string, SoundEffect> Effects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, float> coolDownsMs = new Dictionary<string, float>();
        public static void LoadContent(ContentManager content) {
            Effects.Add("bleep01", content.Load<SoundEffect>("Sounds/bleep01"));
            Effects.Add("bleep02", content.Load<SoundEffect>("Sounds/bleep02"));
        }

        public static void Update(GameTime gameTime) {
            List<string> toRemove = new List<string>();
            for (int i = 0; i < coolDownsMs.Count; i++) {
                string sound = coolDownsMs.ElementAt(i).Key;
                coolDownsMs[sound] -= gameTime.ElapsedGameTime.Milliseconds;
                if (coolDownsMs[sound] <= 0f) {
                    toRemove.Add(sound);
                }
            }
            foreach (string name in toRemove) {
                coolDownsMs.Remove(name);
            }
        }

        public static void PlayEffect(string name) {
            Effects[name].Play();
        }

        public static void PlayEffect(string name, float coolDownMs) {
            if (!coolDownsMs.ContainsKey(name)) {
                PlayEffect(name);
                coolDownsMs[name] = coolDownMs;
            } else {
                Console.WriteLine("Skipping, cooldown");
            }
        }
    }
}
