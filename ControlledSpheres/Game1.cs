using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;

using ControlledSpheres.IO;
using ControlledSpheres.Graphics;

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
        Texture2D Background;
        Creep DebugCreep;
        Creep DebugCreep2;
        Tower DebugTower;
        float DebugRotation = 0f;

        EntityManager MainEntityManager;

        LevelSetup levelSetup;
        public Main()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            SpawnedAnimations = new List<AnimatedGameObject>();
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            levelSetup = new LevelSetup("Content/DebugLevel.txt");
            
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

            MainEntityManager = new EntityManager(levelSetup.ReadPathData());
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
            Background = Content.Load<Texture2D>("Art\\light_sand_template");
            //testLevel = new GameLevel(background);
            this.Window.Position = new Point((graphics.PreferredBackBufferHeight - Background.Height) / 2, (this.graphics.PreferredBackBufferWidth - Background.Width) / 2);
            graphics.PreferredBackBufferHeight = Background.Height / 4 * 3;
            graphics.PreferredBackBufferWidth = Background.Width / 4 * 3;
            graphics.ApplyChanges();
            Texture2D circle = Content.Load<Texture2D>("circle");
            Sphere = new AnimatedGameObject(circle, new Vector2(50, 50));
            DebugAnimation = new Animation(TexManager["ExplosionOneRed"], 20);
            DebugAnimation.Looping = true;
            DebugAnimation.Begin();
            TexManager.requestTextureLoad("ExplosionThreeRed");
            TexManager.requestTextureLoad("ExplosionThreeBlue");
            TexManager.requestTextureLoad("BasicTower");
            TexManager.requestTextureLoad("BasicCreep");
            TexManager.requestTextureLoad("light_sand_template");
            TexManager.requestTextureLoad("BulletSprites");
            TexManager.BeginLoadTextures();
            //  Loader.WriteStandardizedTextures();
            while (TexManager.LoadingTextures == true)
                ;
            TexManager.SplitLaserTexture();
            // While we wait for everything to load - until I implement a proper loading screen
            // Just spin

            DebugCreep = new Creep(new Animation(TexManager["BasicCreep"], 1), new Vector2(100, 100), new Vector2(20f, 20f), 20);
            DebugCreep.RotationZero = new Vector2(0, -1);
            DebugCreep2 = new Creep(DebugCreep.Animations[0], new Vector2(200, 100), new Vector2(50f, 50f), 20);
            DebugCreep2.RotationZero = new Vector2(0, -1);
            DebugTower = new Tower(new Animation(TexManager["BasicTower"], 1), new Vector2(200, 400), 50, .1f);
            DebugTower.AttackType = Projectile.DebugProjectile;
            MainEntityManager.AddCreep(DebugCreep);
            MainEntityManager.AddTower(DebugTower);
            MainEntityManager.AddCreep(DebugCreep2);
            PlayerInputHandler.MouseMovement += MainEntityManager.GetMousePos;
            
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
            DebugRotation += .01f;
            MainEntityManager.Update(gameTime);
            SpawnedAnimations = SpawnedAnimations.Where<AnimatedGameObject>(x => x.Animations[0].Active == true).ToList<AnimatedGameObject>();
            if (TexManager.WaitingRequestedTextures() > 0)
                TexManager.BeginLoadTextures();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            DebugTower.Draw(spriteBatch);
            //DebugTower.Draw(spriteBatch);
            DebugCreep.Draw(spriteBatch);
            DebugCreep2.Draw(spriteBatch);
            MainEntityManager.Draw(spriteBatch);
            foreach (AnimatedGameObject a in SpawnedAnimations) {
                a.Draw(spriteBatch);
            }   
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void HandleButtonPress(object sender, InputStateEventArgs e) {
            Console.WriteLine("Button {0} Pressed, at position {1}", Enum.GetName(typeof(AllButtons), e.Button), e.MousePos.ToString());
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
                case AllButtons.Spacebar:
                    if (e.Modifier.HasFlag(ModifierKeys.Alt))
                        throw new Exception();
                    explColor = "ExplosionThreeGreen";
                    break;
                default:
                    return;
            }
            if (e.Button == AllButtons.Spacebar && e.Modifier.HasFlag(ModifierKeys.Alt))
                throw new Exception();
            Animation[] a = new Animation[1];
            a[0] = new Animation(TexManager[explColor], 30);
            a[0].Begin();
            SpawnedAnimations.Add(new AnimatedGameObject(a, e.MousePos.ToVector2()));
        }

        public void HandleButtonHeld(object sender, InputStateEventArgs e) {
            //Console.WriteLine("Button {0} held at posoition {1}", Enum.GetName(typeof(AllButtons), e.Button), e.MousePos.ToString());
            if (e.Button == AllButtons.MouseButtonLeft)
                MainEntityManager.AddProjectile(DebugTower.Fire(new GameObject(TexManager.BlankTexture[0], e.MousePos.ToVector2())));
            if (e.Button == AllButtons.MouseButtonRight)
                SpawnedAnimations.Add(NewExplosion(e.MousePos.ToVector2(), "ExplosionThreeBlue"));
            //Sphere.Center = e.MousePos.ToVector3();

        }

        /// <summary>
        /// Handles the mouse movement.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="InputStateEventArgs"/> instance containing the event data.</param>
        public void HandleMouseMovement(object sender, InputStateEventArgs e) {
            //Console.WriteLine(e.MouseDelta.ToString());
            //Sphere.Center = e.MousePos.ToVector3();
        }

        /// <summary>
        /// News the explosion.
        /// </summary>
        /// <param name="Position">The position.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public AnimatedGameObject NewExplosion(Vector2 Position, string color) {
            Animation[] Anim = new Animation[1];
            Anim[0] = new Animation(TexManager[color], 40);
            AnimatedGameObject AGO = new AnimatedGameObject(Anim, Position, new Vector2(2, 2));
            AGO.Animations[0].Begin();
            return AGO;
        }

        public Tower NewTower(Vector2 Position) {
            Animation Toweranim = new Animation(TexManager["BasicTower"], 1);
            Animation[] animarr = new Animation[1];
            animarr[0] = Toweranim;
            Tower tower = new Tower(animarr, Position, 50f, 1.5f);
            return tower;
        }

        public Creep NewCreep(Vector2 Position) {
            Animation Creepanim = new Animation(TexManager["BasicCreep"], 1);
            Animation[] animarr = new Animation[1];
            animarr[0] = Creepanim;
            Creep creep = new Creep(animarr, Position, 30);
            return creep;
        }

        public Creep NewCreep(Vector2 Position, Vector2 Velocity) {
            Animation CreepAnimation = new Animation(TexManager["BasicCreep"], 1);
            Animation[] AnimationArray = new Animation[1];
            AnimationArray[0] = CreepAnimation;
            Creep C = new Creep(AnimationArray, Position, Velocity, 30);
            return C;
        }
    }
}
