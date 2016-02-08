using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

using MonoGame.Framework;

using ExtensionMethods;

namespace ControlledSpheres {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
           //GameLevel testLevel;
        AnimatedGameObject Sphere;
        InputHandler PlayerInputHandler;
        InputLogic Keybindings;
        Path DebugPath;

        public Game1()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

            this.IsMouseVisible = true;
            //graphics.IsFullScreen = true;
            //this.Window.IsBorderless = true;
            
            // These 5 lines give me borderless windowed fullscreen
            /*
            IntPtr hWnd = this.Window.Handle;
            var control = System.Windows.Forms.Control.FromHandle(hWnd);
            windowForm = control.FindForm();
            windowForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            windowForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            */

            /*var screen = Screen.AllScreens.First(e => e.Primary);
            Window.IsBorderless = true;
            Window.Position = new Point(screen.Bounds.X, screen.Bounds.Y);
            graphics.PreferredBackBufferWidth = screen.Bounds.Width;
            graphics.PreferredBackBufferHeight = screen.Bounds.Height;
            */Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            PlayerInputHandler = new InputHandler();
            PlayerInputHandler.ButtonPressed += this.HandleButtonPress;
            PlayerInputHandler.MouseMovement += this.HandleMouseMovement;
            Keybindings = new InputLogic();
            DebugPath = new Path(new Vector3(50, 50, 0));
            base.Initialize();
            Console.WriteLine("Program initialized");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D background = Content.Load<Texture2D>("light_sand_template");
            //testLevel = new GameLevel(background);
            this.Window.Position = new Point((graphics.PreferredBackBufferHeight - background.Height) / 2, (this.graphics.PreferredBackBufferWidth - background.Width) / 2);
            graphics.PreferredBackBufferHeight = background.Height / 4 * 3;
            graphics.PreferredBackBufferWidth = background.Width / 4 * 3;
            graphics.ApplyChanges();

            Texture2D circle = Content.Load<Texture2D>("circle");
            Sphere = new AnimatedGameObject(circle, new Vector3(50, 50, 0));
            Sphere.Velocity = new Vector3(0, 3, 0);
            DebugPath.addObject(Sphere);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            PlayerInputHandler.HandleInput();
            DebugPath.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            Sphere.Draw(spriteBatch);
            //testLevel.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void PlacePath(Vector2 Position) {
            DebugPath.addWaypoint(new Waypoint(Position.ToVector3(), 1f));
        }

        public void HandleButtonPress(object sender, InputStateEventArgs e) {
            if (e.Button == AllButtons.MouseButtonLeft)
                Sphere.Position = e.MousePos.ToVector2().ToVector3();
            Console.WriteLine("Button {0} Pressed, at position {1}", Enum.GetName(typeof(AllButtons), e.Button), e.MousePos.ToString());
        }

        public void HandleButtonHeld(object sender, InputStateEventArgs e) {
            if (e.Button == AllButtons.MouseButtonLeft)
                Sphere.Position = e.MousePos.ToVector3();
        }

        public void HandleMouseMovement(object sender, InputStateEventArgs e) {
            Console.WriteLine(e.MouseDelta.ToString());
            Sphere.Position = e.MousePos.ToVector3();
        }
    }
}
