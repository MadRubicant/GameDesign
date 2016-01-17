using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spheres {
    class GraphicalInterface {
        Rectangle screenBounds;
        SpriteFont font;
        GraphicsDevice graphics;
        float[] timeCache;
        int cachePos = 0;
        public Texture2D infoBar;

        public GraphicalInterface(GraphicsDevice graphics, SpriteFont font) {
            timeCache = new float[60];
            timeCache.Select<float, float>(x => 0.0167f);
            this.graphics = graphics;
            this.font = font;
        }
        public void Draw(GameTime gametime, SpriteBatch batch) {
            drawFPS(gametime, batch);
            batch.Draw(infoBar, new Vector2(100, 100), Color.White);
        }

        public void drawFPS(GameTime gameTime, SpriteBatch batch) {
            float time;
            if (gameTime.ElapsedGameTime.Milliseconds == 0)
                time = 0.001f;
            else 
                time = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            timeCache[cachePos] = time;
            cachePos++;
            if (cachePos >= 60)
                cachePos = 0;
            float avgTime = timeCache.Sum() / 60;
            batch.DrawString(font, (1 / avgTime).ToString(), new Vector2(50, 50), Color.White);
        }

    }
}
