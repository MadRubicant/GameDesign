using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    class Creep : DynamicAnimatedGameObject {
        public Creep(Animation[] animationList)
            : base(animationList, new Vector3()) {

        }
    }
}
