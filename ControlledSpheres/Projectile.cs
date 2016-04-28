using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ExtensionMethods;

using ControlledSpheres.Graphics;
namespace ControlledSpheres {
    public class Projectile : AnimatedGameObject {
        public GameObject Target { get; set; }
        public int Damage { get; set; } 

        public Projectile(Animation animation, Vector2 Position, Vector2 Velocity, GameObject Target) :
            base(animation, Position, Velocity) {
                this.Target = Target;
                RotMode = RotationMode.Velocity;
        }

        public override void Update(GameTime gameTime) {
            if (Target != null) {
                Vector2 Direction = Target.Center - this.Center;
            }
            base.Update(gameTime);
        }


        public static Projectile DebugProjectile(GameObject Target, Tower SourceTower) {
            Projectile Proj = new Projectile(new Animation(TextureManager.MainManager["LaserYellow"], 1000), SourceTower.Center, new Vector2(100, 0), Target);
            return Proj;
        }

        
    }

    
}
