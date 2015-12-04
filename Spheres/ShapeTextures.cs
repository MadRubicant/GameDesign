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
            int diam = rad * 2;
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, diam, diam);
            Color[] colorData = new Color[diam * diam];
            float radsq = radius * radius;
            for (int x = 0; x < diam; x++) {
                for (int y = 0; y < diam; y++) {
                    int i = x * diam + y;
                    Vector2 pos = new Vector2(x - rad, y - rad);
                    if (pos.LengthSquared() <= radsq) {
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
