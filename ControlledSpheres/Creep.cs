using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    class Creep : AnimatedGameObject {
        int HP;
        public Creep(Animation[] animationList, Vector3 position, int HP)
            : base(animationList, position) {
            this.HP = HP;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
    }
}
