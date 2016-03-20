using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExtensionMethods {
    public static class ExtensionMethods {
        static float FloatEpsilon = (float)Math.Sin(Math.PI);
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

        /// <summary>
        /// Rotates the current vector to point in the same direction as the Other parameter
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="Other"></param>
        /// <returns>The rotated vector</returns>
        public static Vector2 Rotate(this Vector2 vector, Vector2 Other) {
            Other.Normalize();
            Other *= vector.Length();
            return Other;
        }

        public static Vector2 Rotate(this Vector2 vector, float Angle) {
            float InitialAngle = vector.Angle();
            float NewAngle = InitialAngle + Angle;
            return Vector2.Zero;
        }

        public static bool FuzzyEqual(this Single flt, float other) {
            float diff = Math.Abs(flt - other);
            float relativeDiff = FloatEpsilon * Math.Max(Math.Abs(flt), Math.Abs(other));
            if (diff <= relativeDiff)
                return true;
            else return false;
        }

        public static bool FuzzyEqual(this Vector2 vector, Vector2 other) {
            if (vector.X.FuzzyEqual(other.X) && vector.Y.FuzzyEqual(other.Y))
                return true;
            else return false;
        }
    }
}
