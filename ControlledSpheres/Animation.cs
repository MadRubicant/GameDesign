using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres.Graphics {
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
}
