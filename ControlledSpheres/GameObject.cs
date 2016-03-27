using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ControlledSpheres.Graphics;
using ExtensionMethods;

namespace ControlledSpheres {
    public enum RotationMode { None, Velocity, }
    public class GameObject {
        public RotationMode RotMode { get; set; }
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
        public Vector2 RotationCenter { get; set; }

        #region Constructors
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
            RotationCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
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
            RotationCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public GameObject(Texture2D texture, Vector2 position, Vector2 velocity, RotationMode mode) {
            Texture = texture;
            _center = position;
            Velocity = velocity;
            TexturePosition = new Vector2(Center.X - Texture.Width / 2, Center.Y - Texture.Height / 2);
            Hitbox = texture.Bounds;
            Hitbox.Offset(TexturePosition);
            RotationCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }
        #endregion

        /// <summary>
        /// Updates the specified game time.    
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime) {
            Center += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (RotMode) {
                case RotationMode.Velocity:
                    Rotation = Rotation.Rotate(Velocity);
                    Console.WriteLine(Rotation.Angle());
                    break;
                case RotationMode.None:
                default:
                    break;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
           spriteBatch.Draw(Texture, TexturePosition + RotationCenter, null, Color.White, Rotation.Angle() - RotationZero.Angle(), RotationCenter, 1f, SpriteEffects.None, 0f);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float rotation) {
            spriteBatch.Draw(Texture, RotationCenter + TexturePosition, null, Color.White, rotation - RotationZero.Angle(), RotationCenter, 1f, SpriteEffects.None, 0f);
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
