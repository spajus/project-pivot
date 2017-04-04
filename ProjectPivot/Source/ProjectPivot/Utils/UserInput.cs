using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Utils {
    public static class UserInput {
        static List<Keys> pressedKeys = new List<Keys>();
        public static event KeyHandler OnKeyPressed;
        public static event KeyHandler OnKeyReleased;
        public delegate void KeyHandler(Keys keys);

        public static void Update(GameTime gameTime) {
            KeyboardState kbState = Keyboard.GetState();
            List<Keys> keysToRemove = new List<Keys>();
            foreach(Keys pkey in pressedKeys) {
                if (!kbState.GetPressedKeys().Contains(pkey)) {
                    //released a key
                    if (OnKeyReleased != null) {
                        OnKeyReleased(pkey);
                    }
                    keysToRemove.Add(pkey);
                }
            }
            pressedKeys.RemoveAll(k => keysToRemove.Contains(k));
            List<Keys> keysToAdd = new List<Keys>();
            foreach (Keys key in kbState.GetPressedKeys()) {
                if (!pressedKeys.Contains(key)) {
                    if (OnKeyPressed != null) {
                        OnKeyPressed(key);
                    }
                    keysToAdd.Add(key);
                }
            }
            pressedKeys.AddRange(keysToAdd);
        }
    }
}
