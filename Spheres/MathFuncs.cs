using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace Spheres {
    static class MathFuncs {
        // Returns whether two points in the 2d plane are within distance of each other
        public static bool distanceFormula(Point x, Point y, float distance) {
            Point delta = x - y;
            int total = delta.X * delta.X + delta.Y * delta.Y;
            if (total > distance) {
                return true;
            }
            else return false;
        }
    }
}
