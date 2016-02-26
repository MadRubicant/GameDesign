using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExtensionMethods {
    public static class ExtensionMethods {
        // Returns a Vector2 with the X and Y component of the current Vector3
        public static Vector2 ToVector2(this Vector3 vector) {
            Vector2 ret = new Vector2(vector.X, vector.Y);
            return ret;
        }

        // Returns a Vector3 with components [X, Y, 0] from the current Vector2 
        public static Vector3 ToVector3(this Vector2 vector) {
            Vector3 ret = new Vector3(vector, 0);
            return ret;
        }

        // Returns a Vector3 with components [X, Y, 0] from the current Point
        public static Vector3 ToVector3(this Point point) {
            return new Vector3(point.X, point.Y, 0);
        }

        /// <summary>
        /// Calculates the angle of the vector with respect to the x-axis
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Angle(this Vector2 vector) {
            if (vector == Vector2.Zero)
                return 0f;
            float angle = (float)Math.Atan(vector.Y / vector.X);
            if (Math.Sign(vector.X) < 0)
                angle += MathHelper.Pi;
            return angle;
        }

    }
}
