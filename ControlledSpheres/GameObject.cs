using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ExtensionMethods;

namespace ControlledSpheres {

    
    class GameObject {
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
        public Vector3 Velocity { get; set; }
        public BoundingBox largeBox {get; set;}
        protected BoundingBox[] collisionBox;

        public GameObject() {

        }

        // Position is the center of the object
        public GameObject(Texture2D texture, Vector3 position) {
            Texture = texture;
            _center = position;
            TexturePosition = new Vector2(Center.X - Texture.Width / 2, Center.Y - Texture.Height / 2);
            // The large bounding box is 1 unit deep, .5 above and .5 below
            largeBox = new BoundingBox(Center - new Vector3(-TexturePosition, 1), Center + new Vector3(TexturePosition, 1));
        }

        public GameObject(Texture2D texture, Vector3 position, Vector3 velocity) {
            Texture = texture;
            _center = position;
            Velocity = velocity;
            TexturePosition = new Vector2(Center.X - Texture.Width / 2, Center.Y - Texture.Height / 2);
            // The large bounding box is 1 unit deep, .5 above and .5 below
            largeBox = new BoundingBox(Center - new Vector3(-TexturePosition, 1), Center + new Vector3(TexturePosition, 1));
        }

        public virtual void Update(GameTime gameTime) {
            Center += Velocity;
            largeBox = new BoundingBox(largeBox.Min + Velocity, largeBox.Max + Velocity);
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, TexturePosition, Color.White);
        }
    }  


    class Animation {
        // AnimationSteps[0] should be the inactive state, i.e. standing sprite for an rpg
        public Texture2D[] AnimationSteps;
        public int FrameLength;
        public int CurrentFrame;
        public int TotalFrames;

        public bool Active { get; private set; }
        public bool Looping { get; private set; }

        public Animation(Texture2D[] animationStrip, int framelength, int totalLength) {
            AnimationSteps = animationStrip;
            FrameLength = framelength;
            TotalFrames = totalLength;
            CurrentFrame = 0;
        }

        public void Begin() {
            Active = true;
        }

        // Freezes an animation in place, does NOT reset to stage 0
        public void Freeze() {
            Active = false;
        }

        // Completely resets an animation to state 0 and turns it off
        public void Reset() {
            Active = false;
            CurrentFrame = 0;
        }

        public void Update(GameTime gameTime) {
            // If the animation is active, the frame counter should increment
            if (Active == true) {
                CurrentFrame++;
                if (CurrentFrame == TotalFrames) {
                    CurrentFrame = 0;
                    // Stop if nonlooping animation, continue on otherwise
                    if (Looping == false)
                        Active = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location) {
            spriteBatch.Draw(AnimationSteps[CurrentFrame % FrameLength], location, Color.White);
        }

        public Texture2D getIdleTexture() {
            return AnimationSteps[0];
        }
    }

    // This class is for sprites with multiple possible animations. Like the player in an RPG
    class AnimatedGameObject : GameObject {
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
