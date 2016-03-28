using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ControlledSpheres.Graphics;
namespace ControlledSpheres {
    public delegate Projectile TowerAttack(GameObject Target, Tower tower);

    public class Tower : AnimatedGameObject {
        public float Range { get; set; }
        public float Cooldown { get; set; }
        private float TimeSinceLastShot;
        private bool CanFire = true;
        public TowerAttack AttackType { get; set; }

        public Tower(Animation[] animation, Vector2 position, float range, float CD)
            : base(animation, position) {
                Range = range;
                Cooldown = CD;
        }

        public Tower(Animation animation, Vector2 position, float range, float CD)
            : base(animation, position) {
                Range = range;
                Cooldown = CD;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            TimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeSinceLastShot > Cooldown) {
                CanFire = true;
            }
        }
        public Projectile Fire(GameObject Target) {
            if (AttackType == null) {
                // TODO add logging
                return null;
            }
            else if (CanFire == true) {
                TimeSinceLastShot = 0f;
                CanFire = false;
                return AttackType(Target, this);
            }
            else return null;

        }

        public Projectile Fire(Vector2 GroundTarget) {
            return this.Fire(new GameObject(TextureManager.MainManager.BlankTexture[0], GroundTarget));
        }
        
    }
}
