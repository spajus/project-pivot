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
            if (Settings.ENABLE_AUDIO) {
				Effects.Add("bleep01", content.Load<SoundEffect>("Sounds/bleep01"));
				Effects.Add("bleep02", content.Load<SoundEffect>("Sounds/bleep02"));
				Effects.Add("shot01", content.Load<SoundEffect>("Sounds/shot01"));
				Effects.Add("shot02", content.Load<SoundEffect>("Sounds/shot02"));
				Effects.Add("shot03", content.Load<SoundEffect>("Sounds/shot03"));
				Effects.Add("bullet01", content.Load<SoundEffect>("Sounds/bullet01"));
				Effects.Add("bullet02", content.Load<SoundEffect>("Sounds/bullet02"));
				Effects.Add("bullet03", content.Load<SoundEffect>("Sounds/bullet03"));
            }
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
			if (Settings.ENABLE_AUDIO) {
				Effects[name].Play();
			}
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
