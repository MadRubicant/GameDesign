using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    class Path {
        Dictionary<AnimatedGameObject, Waypoint> PathObjects;
        Waypoint Head;
        Waypoint Tail;
        public Path() {
            PathObjects = new Dictionary<AnimatedGameObject, Waypoint>();
        }


        public void addWaypoint(Waypoint node) {
            if (Head == null)
                Head = Tail = node;
            else {
                Tail.NextNode = node;
            }
        }


        public void addObject(AnimatedGameObject obj) {
            PathObjects.Add(obj, Head);
        }

        public void deleteObject(AnimatedGameObject obj) {
            PathObjects.Remove(obj);
        }

        public bool existsObject(AnimatedGameObject obj) {
            return PathObjects.ContainsKey(obj);
        }

        public void Update(GameTime gameTime) {
            foreach (var obj in PathObjects) {
                obj.Key.Position += obj.Value.getVector() * obj.Value.Velocity;
            }
        }

        
    }

    class Waypoint {
        public Vector3 Position { get; set; }
        public Waypoint NextNode { get; set; }
        public float Velocity { get; set; }
        // Tolerance should be no less than Velocity / 2; otherwise it runs the risk of objects overshooting
        public BoundingSphere Tolerance { get; private set; }


        public Waypoint(Vector3 pos, float velocity, Waypoint next = null) {
            Position = pos;
            Velocity = velocity;
            NextNode = next;
            if (NextNode != null)
                Tolerance = new BoundingSphere(NextNode.Position, velocity);
        }

        // Adds a new waypoint onto the end of this one.
        // =========== IGNORE THIS FOR NOW =============
        // If NextNode != null, then we break the path 
        // Example: 
        /*
         *  Break at the %
         *  ........        ........
         *  xxxx%...        xxxx%%..
         *  ....x...        ....&...
         *  ....x...        ....&...
         *  ....x...        ....&...
         *  The & are now orphaned nodes. Anything already on an & (Is on an & and travelling towards an &) will continue as is
         *  Anything on an x will continue as is
         *  Anything on the % will have its path recalculated to point to the new %
         *  If the path is broken, return true; else return false
         *  =============================================
         *  The above behavior still exists, but is not being used
         *  This function has a return value, but do not rely on it
         */
        public bool registerNext(Waypoint nextWaypoint) {
            if (NextNode == null) {
                NextNode = nextWaypoint;
                Tolerance = new BoundingSphere(NextNode.Position, Velocity);
                return false;
            }
            else {
                NextNode = nextWaypoint;
                Tolerance = new BoundingSphere(NextNode.Position, Velocity);
                return true;
            }
        }

        // Returns the unit vector that points from the current position to the position of the next waypoint
        // If there is no next waypoint, return a 0 vector
        public Vector3 getVector() {
            if (NextNode != null) {
                Vector3 ret = (NextNode.Position - Position);
                ret.Normalize();
                return ret;
            }
            else {
                return new Vector3();
            }
        }

        // Returns whether the game object has arrived at the waypoint
        // "Arrived" means that the center of the object is completely contained in the Tolerance BoundingSphere
        public bool objectArrived(AnimatedGameObject gameObject) {
            var intersect = Tolerance.Contains(gameObject.Position);
            if (intersect == ContainmentType.Contains)
                return true;
            else return false;
            
        }
    }
}
