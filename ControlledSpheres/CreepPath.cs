using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

using ExtensionMethods;

namespace ControlledSpheres {
    class CreepPath {
        public enum WaypointState { }
        Vector2[] WaypointArray;

        public CreepPath(Vector2[] array) {
            WaypointArray = array;
        }

        public bool UpdateCreep(Creep creep, int PathIndex) {
            if (PathIndex >= WaypointArray.Length - 1)
                return false;

            Vector2 WaypointDirection = creep.Velocity;
            Vector2 CreepDirection = WaypointArray[PathIndex] - creep.Center;
            // If the creep is still going in the right direction
            if (Math.Sign(WaypointDirection.X) == Math.Sign(CreepDirection.X) && Math.Sign(WaypointDirection.Y) == Math.Sign(CreepDirection.Y))
                return false;
            CreepDirection = WaypointArray[PathIndex + 1] - creep.Center;
            CreepDirection.Normalize();
            creep.Velocity = creep.Velocity.Rotate(CreepDirection);
            return true;
        }

        public Vector2 Beginning() {
            return WaypointArray[0];
        }
    }
}
