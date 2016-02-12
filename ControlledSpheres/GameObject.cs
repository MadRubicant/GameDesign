using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ExtensionMethods;

namespace ControlledSpheres {

    public class GameObject {
        public Texture2D Texture {get; set;}
        public Vector2 TexturePosition { get; private set; }
        private Vector3 _center;
        public Vector3 Center {
            get {
            return _center;
        }
            set {
                Vector3 diff = _center - TexturePosition.ToVector3();
                _center = value;
                TexturePosition = (_center - diff).ToVector2();
            }
        }

        /// <summary>
        /// Velocity must be in Units per second
        /// </summary>
        public Vector3 Velocity { get; set; }
        public BoundingBox largeBox {get; set;}
        protected BoundingBox[] collisionBox;

        
        public GameObject() {

        }

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public GameObject(Texture2D texture, Vector3 position) {
            Texture = texture;
            _center = position;
            TexturePosition = new Vector2(Center.X - Texture.Width / 2, Center.Y - Texture.Height / 2);
            // The large bounding box is 1 unit deep, .5 above and .5 below
            largeBox = new BoundingBox(Center - new Vector3(-TexturePosition, 1), Center + new Vector3(TexturePosition, 1));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">The texture of this game object</param>
        /// <param name="position">The center position of this game object</param>
        /// <param name="velocity">The velocity of this game object </param>
        public GameObject(Texture2D texture, Vector3 position, Vector3 velocity) {
            Texture = texture;
            _center = position;
            Velocity = velocity;
            TexturePosition = new Vector2(Center.X - Texture.Width / 2, Center.Y - Texture.Height / 2);
            // The large bounding box is 1 unit deep, .5 above and .5 below
            largeBox = new BoundingBox(Center - new Vector3(-TexturePosition, 1), Center + new Vector3(TexturePosition, 1));
        }

        /// <summary>
        /// Updates the specified game time.    
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime) {
            Center += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            largeBox = new BoundingBox(largeBox.Min + Velocity, largeBox.Max + Velocity);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, TexturePosition, Color.Red);
        }
    }  


    public class Animation {
        // AnimationSteps[0] should be the inactive state, i.e. standing sprite for an rpg
        public Texture2D[] AnimationSteps;
        public int AnimationTickLength;
        public int AnimationTotalLength;
        public int AnimationTimeElapsed;
        public int TextureIndex;

        public bool Active { get; private set; }
        public bool Looping { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="animationStrip">The animation strip.</param>
        /// <param name="framelength">The framelength.</param>
        /// <param name="totalLength">The total length.</param>
        public Animation(Texture2D[] animationStrip, int framelength, int totalLength) {
            AnimationSteps = animationStrip;
            AnimationTickLength = framelength;
            AnimationTotalLength = totalLength;
            AnimationTimeElapsed = 0;
            TextureIndex = 0;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="animationStrip">The animation strip.</param>
        /// <param name="framelength">The framelength.</param>
        public Animation(Texture2D[] animationStrip, int framelength) {
            AnimationSteps = animationStrip;
            AnimationTickLength = framelength;
            AnimationTotalLength = AnimationSteps.Length * AnimationTickLength;
            AnimationTimeElapsed = 0;
            TextureIndex = 0;
        }

        /// <summary>
        /// Begins the animation
        /// </summary>
        public void Begin() {
            Active = true;
        }
        
        /// <summary>
        /// Freezes an animation in place, does NOT reset to stage 0
        /// </summary>
        public void Freeze() {
            Active = false;
        }

        /// <summary> 
        /// Completely resets an animation to state 0 and turns it off
        /// </summary>
        public void Reset() {
            Active = false;
            AnimationTimeElapsed = 0;
        }

        /// <summary> 
        /// The animation will stop animating after it finishes the current loop
        /// </summary>
        public void Halt() {
            Looping = false;
        }

        /// <summary>
        /// Updates the animation with the current timing information
        /// </summary>
        /// <param name="gameTime">The amount of time elapsed since the last frame</param>
        public void Update(GameTime gameTime) {
            // If the animation is active, the frame counter should increment
            if (Active == true) {
                AnimationTimeElapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (AnimationTimeElapsed >= (TextureIndex + 1) * AnimationTickLength) {
                    TextureIndex++;
                    if (TextureIndex == AnimationSteps.Length) {
                        TextureIndex = 0;
                        AnimationTimeElapsed = 0;
                        if (Looping == false)
                            Active = false;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the animation's current texture
        /// </summary>
        /// <param name="spriteBatch">Spritebatch</param>
        /// <param name="location">A <see cref="Vector2"/> that determines the location the animation will be drawn to</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 location) {
            spriteBatch.Draw(AnimationSteps[TextureIndex], location);
        }

        /// <summary>
        /// Draws the animation's current texture
        /// </summary>
        /// <param name="spriteBatch">Spritebatch</param>
        /// <param name="location">A <see cref="Vector2"/> that determines the location the animation will be drawn to</param>
        /// <param name="color">The tinting color of the animation</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color) {
            spriteBatch.Draw(AnimationSteps[TextureIndex], location, color);
        }

        public Texture2D getIdleTexture() {
            return AnimationSteps[0];
        }
    }

    // This class is for sprites with multiple possible animations. Like the player in an RPG
    public class AnimatedGameObject : GameObject {
        public Animation[] Animations;
        int currentAnimation = 0;

        public AnimatedGameObject(Animation[] animationList, Vector3 position)
            : base(animationList[0].getIdleTexture(), position) {
            Animations = animationList;
        }

        public AnimatedGameObject(Animation[] animationList, Vector3 position, Vector3 velocity)
            : base(animationList[0].getIdleTexture(), position, velocity) {
                Animations = animationList;
        }
        // This is more of a debugging version to let me spawn an object without making up an animation for it
        public AnimatedGameObject(Texture2D texture, Vector3 position)
            : base(texture, position) {
                Texture2D[] singleTexture = new Texture2D[1];
                singleTexture[0] = texture;
                Animations = new Animation[1];
                Animations[0] = new Animation(singleTexture, 1, 1);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Animations[currentAnimation].Draw(spriteBatch, this.TexturePosition);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            Animations[currentAnimation].Update(gameTime);
        }
    }
}
