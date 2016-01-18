using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    class Path {
        HashSet<DynamicAnimatedGameObject> PathObjects;
        PathNode Head;

        public Path() {
            PathObjects = new HashSet<DynamicAnimatedGameObject>();
        }


        public void addNode(PathNode node) {
            if (Head == null) {
                Head = node;
            }
            else {
                // It's a linked list. What else do you want
                PathNode next = Head.NextNode;
                while (Head.NextNode != null)
                    next = Head.NextNode;
                next.NextNode = node;
            }
        }


        public void addObject(DynamicAnimatedGameObject obj) {
            PathObjects.Add(obj);
        }

        public void deleteObject(DynamicAnimatedGameObject obj) {
            PathObjects.Remove(obj);
        }
    }

    class PathNode {
        public Vector3 Position { get; set; }
        public PathNode NextNode { get; set; }


    }
}
