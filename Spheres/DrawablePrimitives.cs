using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Spheres {
    class Primitive : Drawable{
        public Primitive(Shape shape, Texture2D tex) {
            type = shape;
            texture = tex;
            mass = 200;
        }
        Shape type;
        Texture2D texture;
        float mass;
        public Shape shapeType {
            get {
                return type;
            }
            set {
                type = value;
            }
        }

        public Texture2D Texture {
            get {
                return texture;
            }
            set {
                texture = value;
            }
        }

        public float Mass {
            get {
                return mass;
            }
        }
        public void Draw(SpriteBatch batch) {
            batch.Draw(texture, type.TexturePosition(), Color.White);
        }

        public void Update(GameTime gameTime) {
            type.Position += type.Velocity;
        }
    }
}
