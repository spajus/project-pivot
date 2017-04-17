using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPivot.Entities;

namespace ProjectPivot.Utils {
    public class FPSCounter {
        private SpriteFont font;
        public double AverageFPS { get; private set; }
        public double CurrentFPS { get; private set; }
        public const int MAX_SAMPLES = 100;

        private Queue<double> sampleBuffer = new Queue<double>();

        public void LoadContent(ContentManager content) {
            font = content.Load<SpriteFont>("Fonts/manaspc");
        }

        public void Update(GameTime gameTime) {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            CurrentFPS = 1.0d / deltaTime;
            sampleBuffer.Enqueue(CurrentFPS);
            if (sampleBuffer.Count > MAX_SAMPLES) {
                sampleBuffer.Dequeue();
                AverageFPS = sampleBuffer.Average(i => i);
            } else {
                AverageFPS = CurrentFPS;
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, Camera camera) {
            Vector2 position = Settings.DEBUG_POSITION;
            var fps = string.Format("FPS: {0}", Math.Round(AverageFPS));
            Vector2 bgSize = font.MeasureString(fps);
            spriteBatch.Draw(Gizmo.Pixel,
                camera.ToWorldCoordinates(position),
                new Rectangle(0, 0, (int) bgSize.X, (int) bgSize.Y), 
               Color.Black, 0f, Vector2.Zero, Vector2.One / Camera.Main.Zoom, 
               SpriteEffects.None, 0.1f);
            spriteBatch.DrawString(
                font, fps, 
                camera.ToWorldCoordinates(position), 
                Color.Green, 0f, Vector2.Zero, 
                Vector2.One / Camera.Main.Zoom, SpriteEffects.None, 0);
        }
    }
}
