using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spheres {
    abstract class Shape {
        // Merely here to tie shapes together
        public virtual Vector2 Position {
            get;
            set;
        }
        public virtual Vector2 Velocity {
            get;
            set;
        }
        public abstract Vector2 TexturePosition();
        public abstract bool Intersects(Shape other);
    }
    class Circle : Shape {
        float radius;
        Vector2 position;
        Vector2 velocity;

        public Circle() {
            // Implicitly initialize everything to 0. It's a stationary circle at position 0. Kind of useless
        }

            //Stationary circle at the given position
        public Circle(float rad, Vector2 pos) {
            radius = rad;
            position = pos;
            velocity = new Vector2();
        }
        // Moving circle with the given parameters
        public Circle(float rad, Vector2 pos, Vector2 vel) {
            radius = rad;
            position = pos;
            velocity = vel;
        }

        public override Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
            }
        }
        
        public override Vector2 Velocity {
            get {
                return velocity;
            }
            set {
                velocity = value;
            }
        }

        public override Vector2 TexturePosition() {
            Vector2 pos = position;
            pos.X -= radius / 2;
            pos.Y -= radius / 2;
            return pos;
        }

        public override bool Intersects(Shape other) {
            /* xdiff is the difference in the x component of the vectors
             * ydiff is the difference in y
             * radsum is the sum of the radii
             * Using the distance formula without the square roots because performance
             * dx^2 + dy^2 > r^2 means that the two circles aren't intersecting
             * dx^2 + dy^2 <= r^2 means they're intersecting
             */
            Circle c = other as Circle;
            // Add their velocities to their positions
            Vector2 changePos = c.Position - position;
            float radsum = radius + c.radius;
            //changePos += velocity + c.velocity;

            if (changePos.LengthSquared() <= radsum * radsum)
                return true;
            else
                return false;
        }
    }
}
