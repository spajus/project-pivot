using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectPivot.Components;
using ProjectPivot.Entities;
using ProjectPivot.Utils;
using System;

namespace ProjectPivot
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ProjectPivot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Player player;
        FPSCounter fpsCounter;
        Map map;
        public static World World { get; protected set; }

        public static Texture2D DebugPixel;
        
        public ProjectPivot()
        {
            World = new World(Vector2.Zero);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player(Vector2.Zero);
            GameObjects.Add(player);
            camera = new Camera(GraphicsDevice.Viewport);
            Camera.Main = camera;
            camera.Target = player;
            GameObjects.Add(camera);
			Gizmo.Initialize(GraphicsDevice);
            //GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            fpsCounter = new FPSCounter();

            map = new Map(300, 300);
            map.Generate();
            base.Initialize();
            this.IsFixedTimeStep = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fpsCounter.LoadContent(Content);
            Textures.LoadContent(Content);
            CellGraphics.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // variable time step but never less then 30 Hz
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            camera.Update(gameTime);
            player.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, 
                null, null, null, null, camera.Transform);

            base.Draw(gameTime);

            fpsCounter.Update(gameTime);
            fpsCounter.Draw(spriteBatch, camera);

            GameObjects.Draw(spriteBatch);

            if (false) { //debug
                spriteBatch.Draw(DebugPixel, camera.VisibleArea, Color.Red);
            }

			Gizmo.Rectangle(new Rectangle(5, 5, 200, 100));
			Gizmo.Draw(spriteBatch);

			spriteBatch.End();
        }
    }
}
