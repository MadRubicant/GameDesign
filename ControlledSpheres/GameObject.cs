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
        private Vector2 _center;
        public Vector2 Center {
            get {
            return _center;
        }
            set {
                Vector2 diff = _center - TexturePosition;
                _center = value;
                TexturePosition = (_center - diff);
                Hitbox.Offset(diff);
            }
        }

        /// <summary>
        /// Velocity must be in Units per second
        /// </summary>
        public Vector2 Velocity { get; set; }
        public Rectangle Hitbox {get; private set;}
        public Vector2 RotationZero { get; set; }
        public Vector2 Rotation { get; set; }
        
        #region Constructors
        public GameObject() {

        }

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public GameObject(Texture2D texture, Vector2 position) {
            Texture = texture;
            _center = position;
            TexturePosition = new Vector2(Center.X - Texture.Width / 2, Center.Y - Texture.Height / 2);
            Hitbox = texture.Bounds;
            Hitbox.Offset(TexturePosition);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">The texture of this game object</param>
        /// <param name="position">The center position of this game object</param>
        /// <param name="velocity">The velocity of this game object </param>
        public GameObject(Texture2D texture, Vector2 position, Vector2 velocity) {
            Texture = texture;
            _center = position;
            Velocity = velocity;
            TexturePosition = new Vector2(Center.X - Texture.Width / 2, Center.Y - Texture.Height / 2);
            Hitbox = texture.Bounds;
            Hitbox.Offset(TexturePosition);
            // The large bounding box is 1 unit deep, .5 above and .5 below
        }
        #endregion

        /// <summary>
        /// Updates the specified game time.    
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime) {
            Center += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, TexturePosition, Color.White);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float Rotation) {
            Vector2 RotationCenter = new Vector2(this.Texture.Width, this.Texture.Height);
            spriteBatch.Draw(Texture, RotationCenter + TexturePosition, null, Color.White, Rotation - RotationZero.Angle(), RotationCenter / 2, 1f, SpriteEffects.None, 0f);
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
            spriteBatch.Draw(AnimationSteps[TextureIndex], location, Color.White);
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

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color, float Rotation) {
            Vector2 RotationCenter = new Vector2(this.AnimationSteps[TextureIndex].Width, this.AnimationSteps[TextureIndex].Height) / 2f;
            spriteBatch.Draw(this.AnimationSteps[TextureIndex], location + RotationCenter, null, Color.White, Rotation, RotationCenter, 1f, SpriteEffects.None, 0f);
        }

        public Texture2D getIdleTexture() {
            return AnimationSteps[0];
        }
    }

    // This class is for sprites with multiple possible animations. Like the player in an RPG
    public class AnimatedGameObject : GameObject {
        public Animation[] Animations;
        int currentAnimation = 0;
        // A normalized vector that gives the rotation with respect to
        #region Constructors
        public AnimatedGameObject(Animation[] animationList, Vector2 position)
            : base(animationList[0].getIdleTexture(), position) {
            Animations = animationList;
        }

        public AnimatedGameObject(Animation[] animationList, Vector2 position, Vector2 velocity)
            : base(animationList[0].getIdleTexture(), position, velocity) {
                Animations = animationList;
        }

        public AnimatedGameObject(Animation animation, Vector2 position)
            : base(animation.getIdleTexture(), position) {
            Animations = new Animation[1];
            Animations[0] = animation;
        }

        public AnimatedGameObject(Animation animation, Vector2 position, Vector2 velocity)
            : base(animation.getIdleTexture(), position, velocity) {
            Animations = new Animation[1];
            Animations[0] = animation;
        }

        // This is more of a debugging version to let me spawn an object without making up an animation for it
        public AnimatedGameObject(Texture2D texture, Vector2 position)
            : base(texture, position) {
                Texture2D[] singleTexture = new Texture2D[1];
                singleTexture[0] = texture;
                Animations = new Animation[1];
                Animations[0] = new Animation(singleTexture, 1, 1);
        }
        #endregion
        public override void Draw(SpriteBatch spriteBatch) {
            Animations[currentAnimation].Draw(spriteBatch, this.TexturePosition, Color.White, Rotation.Angle() - RotationZero.Angle());
        }

        public override void Draw(SpriteBatch spriteBatch, float Rotation) {
            Animations[currentAnimation].Draw(spriteBatch, this.TexturePosition, Color.White, Rotation);
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            Animations[currentAnimation].Update(gameTime);
        }
    }
}
