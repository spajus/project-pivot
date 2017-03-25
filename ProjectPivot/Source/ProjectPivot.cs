//#define WINDOWS

using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectPivot.Components;
using ProjectPivot.Entities;
using ProjectPivot.Rendering;
using ProjectPivot.Utils;
using System;

namespace ProjectPivot {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ProjectPivot : Game {
        // CONSTANTS
        public SamplerState globalSamplerState = SamplerState.PointWrap;
        public const double minPhysicsStepTime = 1.0 / 30.0;
        public const bool physicsDebugEnabled = false;
        public const bool cellsDebugEnabled = false;
        public const bool mapDebugEnabled = false;
        public const bool gizmosEnabled = true;
        public const bool gizmoGridEnabled = false;
        public const int mapWidth = 30;
        public const int mapHeight = 30;
        public const int screenWidth = 1200;
        public const int screenHeight = 800;

        public Effect GlobalShader = null;

        BloomComponent bloom;
        int bloomSettingsIndex = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Player player;
        FPSCounter fpsCounter;
        Map map;
        PhysicsDebug physicsDebug;


        public static World World { get; protected set; }

        public static Texture2D DebugPixel;

        public ProjectPivot() {
            World = new World(Vector2.Zero);

            if (physicsDebugEnabled) {
                physicsDebug = new PhysicsDebug(World);
            }
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;

#if WINDOWS
            bloom = new BloomComponent(this);
            Components.Add(bloom);
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            for (int i = 0; i < 16; i++) {
                GraphicsDevice.SamplerStates[0] = globalSamplerState;
            }

            Gizmo.Initialize(GraphicsDevice);
            Weapons.Initialize();
            //GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            fpsCounter = new FPSCounter();

            map = new Map(mapWidth, mapHeight, Vector2.Zero);
            Map.Current = map;
            GameObjects.Initialize(map);
            map.Generate();
            player = new Player(map.RandomHollowCell().Position);
            GameObjects.Add(player, true);

            Enemy enemy1 = new Enemy(map.RandomHollowCell().Position);
            Enemy enemy2 = new Enemy(map.RandomHollowCell().Position);
            enemy1.Target = player;
            enemy2.Target = player;
            //Enemy enemy3 = new Enemy(map.RandomHollowCell().Position);

            GameObjects.Add(enemy1, true);
            GameObjects.Add(enemy2, true);
            //GameObjects.Add(enemy3, true);

            Weapon w = Weapons.Build("sniper_rifle");
            player.TakeWeapon(w);

            GameObjects.Add(w, true);

            camera = new Camera(GraphicsDevice.Viewport, player.Position);
            Camera.Main = camera;

            camera.Target = player;
            GameObjects.Add(camera, true);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            Gizmo.LoadContent(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fpsCounter.LoadContent(Content);
            Textures.LoadContent(Content);
            CellGraphics.LoadContent(Content);
            if (physicsDebugEnabled) {
                physicsDebug.LoadContent(GraphicsDevice, Content);
            }

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

#if WINDOWS
            if (Keyboard.GetState().IsKeyDown(Keys.X)) {
                bloom.Visible = false;
            } else {
                bloom.Visible = true;
            }
            //bloom.ShowBuffer++
#endif

            base.Update(gameTime);

            // variable time step but never less then 30 Hz
            double frameTime = gameTime.ElapsedGameTime.TotalSeconds;
            World.Step((float) Math.Min(frameTime, minPhysicsStepTime));
			GameObjects.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (bloom != null) {
                bloom.BeginDraw();
            }
            GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			spriteBatch.Begin(SpriteSortMode.BackToFront,
                              BlendState.AlphaBlend,
                              globalSamplerState,
                              DepthStencilState.Default,
                              RasterizerState.CullNone,
                              GlobalShader,
                camera.Transform);


            fpsCounter.Update(gameTime);
            fpsCounter.Draw(spriteBatch, camera);

            GameObjects.Draw(spriteBatch);

            if (false) { //debug
                spriteBatch.Draw(DebugPixel, camera.VisibleArea, Color.Red);
            }

			//Gizmo.Rectangle(new Rectangle(5, 5, 200, 100));
            Gizmo.Draw(spriteBatch, gizmoGridEnabled);
			spriteBatch.End();
            base.Draw(gameTime);

            
            if (physicsDebugEnabled) {
                physicsDebug.Draw();
            }
        }
    }
}
