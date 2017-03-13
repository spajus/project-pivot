using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public static Texture2D DebugPixel;
        
        public ProjectPivot()
        {
            graphics = new GraphicsDeviceManager(this);
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
            camera = new Camera(GraphicsDevice.Viewport);
            camera.Target = player;
            fpsCounter = new FPSCounter();
            DebugPixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            DebugPixel.SetData(new[] { Color.White });
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
            Cell.LoadContent(Content);

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
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
                null, null, null, null, camera.Transform);

            base.Draw(gameTime);
            map.Draw(camera, spriteBatch);
            fpsCounter.Update(gameTime);
            fpsCounter.Draw(spriteBatch, camera);

            player.Draw(spriteBatch);

            if (false) { //debug
                spriteBatch.Draw(DebugPixel, camera.VisibleArea, Color.Red);
            }

			spriteBatch.End();
        }
    }
}
