using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;

using ExtensionMethods;

using ControlledSpheres.IO;
namespace ControlledSpheres {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
           //GameLevel testLevel;
        AnimatedGameObject Sphere;
        InputHandler PlayerInputHandler;
        InputLogic Keybindings;

        Animation DebugAnimation;
        List<AnimatedGameObject> SpawnedAnimations;
        TextureManager TexManager;

        public Main()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            SpawnedAnimations = new List<AnimatedGameObject>();
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
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
            PlayerInputHandler.ButtonHeld += this.HandleButtonHeld;
            Keybindings = new InputLogic();
            base.Initialize();
            Console.WriteLine("Program initialized");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            TexManager = new TextureManager(Content, GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D background = Content.Load<Texture2D>("light_sand_template");
            //testLevel = new GameLevel(background);
            this.Window.Position = new Point((graphics.PreferredBackBufferHeight - background.Height) / 2, (this.graphics.PreferredBackBufferWidth - background.Width) / 2);
            graphics.PreferredBackBufferHeight = background.Height / 4 * 3;
            graphics.PreferredBackBufferWidth = background.Width / 4 * 3;
            graphics.ApplyChanges();

            Texture2D circle = Content.Load<Texture2D>("circle");
            Sphere = new AnimatedGameObject(circle, new Vector3(50, 50, 0));
            DebugAnimation = new Animation(TexManager["ExplosionOneRed"], 20);
            DebugAnimation.Looping = true;
            DebugAnimation.Begin();
            //TexManager.LoadAllTextures();
            TexManager.LoadSpecificTexture("ExplosionThreeRed");
            TexManager.LoadSpecificTexture("ExplosionThreeBlue");
            //  Loader.WriteStandardizedTextures();
            


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
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
            // TODO: Add your update logic here
            Sphere.Update(gameTime);
            DebugAnimation.Update(gameTime);
            foreach (AnimatedGameObject a in SpawnedAnimations) {
                a.Update(gameTime);
            }
            SpawnedAnimations = SpawnedAnimations.Where<AnimatedGameObject>(x => x.Animations[0].Active == true).ToList<AnimatedGameObject>();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            //Sphere.Draw(spriteBatch);
            //DebugAnimation.Draw(spriteBatch, new Vector2(50, 50));
            foreach (AnimatedGameObject a in SpawnedAnimations) {
                a.Draw(spriteBatch);
            }
            //spriteBatch.Draw(TextureManager.TextureAtlas[TextureNames.Debug][0], new Vector2(), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void HandleButtonPress(object sender, InputStateEventArgs e) {
            string explColor = "debug";
            switch (e.Button) {
                case AllButtons.Q:
                    explColor = "ExplosionThreeRedDown";
                    break;
                case AllButtons.W:
                    explColor = "ExplosionThreeRedLeft";
                    break;
                case AllButtons.E:
                    explColor = "ExplosionThreeRedRight";
                    break;
                case AllButtons.R:
                    explColor = "ExplosionThreeRedUp";
                    break;
                case AllButtons.A:
                    explColor = "ExplosionThreeBlueDown";
                    break;
                case AllButtons.S:
                    explColor = "ExplosionThreeBlueLeft";
                    break;
                case AllButtons.D:
                    explColor = "ExplosionThreeBlueRight";
                    break;
                case AllButtons.F:
                    explColor = "ExplosionThreeBlueUp";
                    break;
                default:
                    return;
            }
            Animation[] a = new Animation[1];
            a[0] = new Animation(TexManager[explColor], 30);
            a[0].Begin();
            SpawnedAnimations.Add(new AnimatedGameObject(a, e.MousePos.ToVector3()));
            Console.WriteLine("Button {0} Pressed, at position {1}", Enum.GetName(typeof(AllButtons), e.Button), e.MousePos.ToString());
        }

        public void HandleButtonHeld(object sender, InputStateEventArgs e) {
            Console.WriteLine("Button {0} held at posoition {1}", Enum.GetName(typeof(AllButtons), e.Button), e.MousePos.ToString());
            if (e.Button == AllButtons.MouseButtonLeft)
                SpawnedAnimations.Add(NewExplosion(e.MousePos.ToVector3(), "ExplosionThreeRed"));
            if (e.Button == AllButtons.MouseButtonRight)
                SpawnedAnimations.Add(NewExplosion(e.MousePos.ToVector3(), "ExplosionThreeBlue"));
                //Sphere.Center = e.MousePos.ToVector3();
            
        }

        /// <summary>
        /// Handles the mouse movement.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="InputStateEventArgs"/> instance containing the event data.</param>
        public void HandleMouseMovement(object sender, InputStateEventArgs e) {
            Console.WriteLine(e.MouseDelta.ToString());
            //Sphere.Center = e.MousePos.ToVector3();
        }

        /// <summary>
        /// News the explosion.
        /// </summary>
        /// <param name="Position">The position.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public AnimatedGameObject NewExplosion(Vector3 Position, string color) {
            Animation[] Anim = new Animation[1];
            Anim[0] = new Animation(TexManager[color], 40);
            AnimatedGameObject AGO = new AnimatedGameObject(Anim, Position, new Vector3(2, 2, 0));
            AGO.Animations[0].Begin();
            return AGO;
        }
    }
}
