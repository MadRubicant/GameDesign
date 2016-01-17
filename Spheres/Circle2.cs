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
        BoundingSphere collisionSphere;
        Vector3 velocity;

        public Circle() {
            // Implicitly initialize everything to 0. It's a stationary circle at position 0. Kind of useless
        }

            //Stationary circle at the given position
        public Circle(float rad, Vector2 pos) {
            collisionSphere = new BoundingSphere(new Vector3(pos, 0), rad);
            velocity = new Vector3();
        }
        // Moving circle with the given parameters
        public Circle(float rad, Vector2 pos, Vector2 vel) {
            collisionSphere = new BoundingSphere(new Vector3(pos, 0), rad);
            velocity = new Vector3(vel, 0);
        }

        public override Vector2 Position {
            get {
                return new Vector2(collisionSphere.Center.X, collisionSphere.Center.Y);
            }
            set {
                collisionSphere.Center = new Vector3(value, 0);
            }
        }
        
        public override Vector2 Velocity {
            get {
                return new Vector2(velocity.X, velocity.Y);
            }
            set {
                velocity = new Vector3(value, 0);
            }
        }

        public override Vector2 TexturePosition() {
            Vector2 pos = new Vector2(collisionSphere.Center.X, collisionSphere.Center.Y);
            pos.X -= collisionSphere.Radius;
            pos.Y -= collisionSphere.Radius;
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
            if (c.collisionSphere.Intersects(collisionSphere)) {
                if (c.collisionSphere.Contains(collisionSphere) == ContainmentType.Contains || collisionSphere.Contains(c.collisionSphere) == ContainmentType.Contains)
                    return false;
                return true;
            }
            else return false;
        }
    }
}
