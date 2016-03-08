using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    public class Creep : AnimatedGameObject {
        int HP;

        
        public Creep(Animation[] animationList, Vector2 position, int HP)
            : base(animationList, position) {
            this.HP = HP;
        }

        public Creep(Animation[] animationlist, Vector2 position, Vector2 velocity, int HP)
            : base(animationlist, position, velocity) {
            this.HP = HP;
        }

        public Creep(Animation animation, Vector2 position, int HP)
            : base(animation, position) {
            this.HP = HP;
        }

        public Creep(Animation animation, Vector2 position, Vector2 velocity, int HP)
            : base(animation, position, velocity) {
            this.HP = HP;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
    }
}
