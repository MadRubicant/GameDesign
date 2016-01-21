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
        public Vector3 Position {get; set;}
        public BoundingBox largeBox {get; set;}
        protected BoundingBox[] collisionBox;

        public GameObject() {

        }
        public GameObject(Texture2D texture, Vector3 position) {
            Texture = texture;
            Position = position;
            // The large bounding box is 1 unit deep, .5 above and .5 below
            largeBox = new BoundingBox(Position - new Vector3(0, 0, 1), Position + new Vector3(Texture.Width, Texture.Height, 1));
        }
        public virtual void Update(GameTime gameTime) {

        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position.ToVector2(), Color.White);
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

        // This is more of a debugging version to let me spawn an object without making up an animation for it
        public AnimatedGameObject(Texture2D texture, Vector3 position)
            : base(texture, position) {
                Texture2D[] singleTexture = new Texture2D[1];
                singleTexture[0] = texture;
                Animations = new Animation[1];
                Animations[0] = new Animation(singleTexture, 1, 1);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Animations[currentAnimation].Draw(spriteBatch, this.Position.ToVector2());
        }
    }
}
