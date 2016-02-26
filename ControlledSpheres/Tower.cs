using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {

    public interface GameEntity {

    }

    public class Tower : AnimatedGameObject {
        public Tower(Animation[] animation, Vector2 position)
            : base(animation, position) {
                ;
        }

        public Tower(Animation animation, Vector2 position)
            : base(animation, position) {
                ;
        }

        
    }
}
