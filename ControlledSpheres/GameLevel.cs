using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ExtensionMethods;

namespace ControlledSpheres {
    class GameLevel {
        Texture2D Background;
        Path CreepPath;

        public GameLevel(Texture2D Background) {
            this.Background = Background;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Background, new Vector2(), Background.Bounds, Color.White, 0, new Vector2(), .75f, SpriteEffects.None, 0f);
        }

        // Level file format: an integer n that specifies how many nodes are in the path; then n single precision floats that describe the path
        public void loadFromFile(string filename) {

        }
    }
}
