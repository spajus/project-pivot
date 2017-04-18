using Microsoft.Xna.Framework;
using ProjectPivot.Components;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ProjectPivot.Utils;

namespace ProjectPivot.UI {
    public class HealthBar : UIElement {
        Player player;
        Rectangle healthRect;
        Rectangle damageRect;
        public float health;
        public HealthBar(Rectangle position, Player player) : base("health", "", position)  {
            this.player = player;
            this.health = player.GetComponent<Health>().Value;
            damageRect = new Rectangle(
                position.X + 1, position.Y + 1,
                position.Width - 2,
                position.Height - 2);
            createHealthRect();
        }

        private void createHealthRect() {
            healthRect = new Rectangle(
                position.X + 1, position.Y + 1,
                (int) (position.Width * MathHelper.Clamp(health / 100f, 0, 1)) - 2,
                position.Height - 2);
        }

        public override void Update(GameTime gameTime) {
            float actualHealth = player.GetComponent<Health>().Value;
            if ((int) actualHealth != (int) health) {
                health = actualHealth;
                createHealthRect();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera) {
            spriteBatch.Draw(Gizmo.Pixel, 
                camera.ToWorldCoordinates(position.X, position.Y), 
                new Rectangle(0, 0, position.Width, position.Height),
                Color.Black, 0f, Vector2.Zero, Vector2.One / camera.Zoom, SpriteEffects.None, 0.9998f);
            spriteBatch.Draw(Gizmo.Pixel, 
                camera.ToWorldCoordinates(damageRect.X, damageRect.Y), 
                new Rectangle(0, 0, damageRect.Width, damageRect.Height),
                Color.Red, 0f, Vector2.Zero, Vector2.One / camera.Zoom, SpriteEffects.None, 0.9997f);
            spriteBatch.Draw(Gizmo.Pixel, 
                camera.ToWorldCoordinates(healthRect.X, healthRect.Y), 
                new Rectangle(0, 0, healthRect.Width, healthRect.Height),
                Color.Green, 0f, Vector2.Zero, Vector2.One / camera.Zoom, SpriteEffects.None, 0.9996f);
        }
    }
}
