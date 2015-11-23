using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spheres {
    static class ShapeTextures {
        public static GraphicsDeviceManager graphics;

        // MUST be called before trying to make any shapes
        public static void initGraphics(GraphicsDeviceManager g) {
            graphics = g;
        }

        public static Texture2D Circle(float radius, Color color = new Color()) {
            int rad = (int)radius;
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, rad, rad);
            Color[] colorData = new Color[rad * rad];
            float diam = radius / 2f;
            float diamsq = diam * diam;
            for (int x = 0; x < rad; x++) {
                for (int y = 0; y < rad; y++) {
                    int i = x * rad + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq) {
                        colorData[i] = color;
                    }
                    else {
                        colorData[i] = Color.Transparent;
                    }
                }
            }
            texture.SetData<Color>(colorData);
            return texture;
        }
    }
}
