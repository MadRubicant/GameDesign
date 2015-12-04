using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
//using System.Drawing;
using System.Runtime.InteropServices;

//using System.Windows.Forms;

using MonoGame.Framework;
namespace Spheres {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Point Center;
        Rectangle screen;
        List<Primitive> Circles;
        Random rand = new Random();
        GraphicalInterface gui;
        public Game1()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            graphics.IsFullScreen = true;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.TitleSafeArea.Height;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.TitleSafeArea.Width;
            graphics.ApplyChanges();
            screen = graphics.GraphicsDevice.Viewport.TitleSafeArea;
            Center = new Point(screen.Width / 2, screen.Height / 2);
            Circles = new List<Primitive>();
            base.Initialize();
        }
 
        

        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ShapeTextures.initGraphics(graphics);
            for (int i = 0; i < 500; i++) {
                var circ = genCircle();
                bool good = true;
                foreach (Primitive p in Circles) {
                    if (p.shapeType.Intersects(circ.shapeType))
                        good = false;   
                }
                if (good == true)
                    Circles.Add(circ);
                else
                    i--;
            }
            font = Content.Load<SpriteFont>("mainFont");
            gui = new GraphicalInterface(graphics.GraphicsDevice, font);

            // TODO: use this.Content to load your game content here
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
            foreach (Primitive p in Circles) {
                p.Update(gameTime);
            }
            Collisions();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (Primitive p in Circles) {
                p.Draw(spriteBatch);
            }
            gui.drawFPS(gameTime, spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }

        void Collisions() {
            HashSet<Tuple<Primitive, Primitive>> foundCollisions = new HashSet<Tuple<Primitive, Primitive>>();
            for (int i = 0; i < Circles.Count; i++) {
                for (int j = 0; j < Circles.Count; j++) {
                        if (i == j)
                            continue;
                    if (Circles[j].shapeType.Intersects(Circles[i].shapeType) && !foundCollisions.Contains(new Tuple<Primitive,Primitive>(Circles[j], Circles[i]))) {
                        foundCollisions.Add(new Tuple<Primitive, Primitive>(Circles[i], Circles[j]));
                        var a = Circles[j];
                        var b = Circles[i];
                        // Following http://www.vobarian.com/collisions/2dcollisions2.pdf
                        // "Mass" is radius
                        Vector2 unitNormal = a.shapeType.Position - b.shapeType.Position;
                        Vector2 unitTangent;
                        unitNormal.Normalize();
                        unitTangent.X = -unitNormal.Y;
                        unitTangent.Y = unitNormal.X;
                        float vNormA, vNormB, vTanA, vTanB;
                        vNormA = Vector2.Dot(a.shapeType.Velocity, unitNormal);
                        vNormB = Vector2.Dot(b.shapeType.Velocity, unitNormal);
                        vTanA = Vector2.Dot(a.shapeType.Velocity, unitTangent);
                        vTanB = Vector2.Dot(b.shapeType.Velocity, unitTangent);
                        float vNormAp = (vNormA * (a.Mass - b.Mass) + 2 * b.Mass * vNormB) / (a.Mass + b.Mass);
                        float vNormBp = (vNormB * (b.Mass - a.Mass) + 2 * a.Mass * vNormA) / (a.Mass + b.Mass);
                        a.shapeType.Velocity = vNormAp * unitNormal + vTanA * unitTangent;
                        b.shapeType.Velocity = vNormBp * unitNormal + vTanB * unitTangent;
                    }
                }
                Primitive p = Circles[i];
                if (!screen.Contains(p.shapeType.Position)) {
                    if (p.shapeType.Position.X > screen.Width && p.shapeType.Velocity.X > 0)
                        p.shapeType.Velocity = new Vector2(-p.shapeType.Velocity.X, p.shapeType.Velocity.Y);
                    if (p.shapeType.Position.X < 0 && p.shapeType.Velocity.X < 0)
                        p.shapeType.Velocity = new Vector2(-p.shapeType.Velocity.X, p.shapeType.Velocity.Y);
                    if (p.shapeType.Position.Y > screen.Height && p.shapeType.Velocity.Y > 0)
                        p.shapeType.Velocity = new Vector2(p.shapeType.Velocity.X, -p.shapeType.Velocity.Y);
                    if (p.shapeType.Position.Y < 0 && p.shapeType.Velocity.Y < 0)
                        p.shapeType.Velocity = new Vector2(p.shapeType.Velocity.X, -p.shapeType.Velocity.Y);

                }
            }
        }

        Primitive genCircle() {
            float x = (float)rand.NextDouble() * screen.Width;
            float y = (float)rand.NextDouble() * screen.Height;
            float dx = (float)rand.NextDouble() * 5;
            float dy = (float)rand.NextDouble() * 5;
            float radius = 25;
            Color c = new Color(rand.Next(5, 255), rand.Next(5, 255), rand.Next(5, 255));
            Primitive p = new Primitive(new Circle(radius, new Vector2(x, y), new Vector2(dx, dy)), ShapeTextures.Circle(radius, c));
            
            return p;
        }
    }
}
