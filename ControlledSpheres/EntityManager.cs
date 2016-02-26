using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    class EntityManager {
        HashSet<Tower> ActiveTowers;
        HashSet<Creep> ActiveCreeps;

        bool MouseChanged = false;
        Vector2 MousePos;
        public EntityManager() {
            ActiveTowers = new HashSet<Tower>();
            ActiveCreeps = new HashSet<Creep>();
        }

        public void Update(GameTime gameTime) {
            foreach (Creep c in ActiveCreeps) {
                c.Rotation = c.Center - MousePos;
            }
            foreach (Tower t in ActiveTowers) {
                t.Rotation = t.Center - MousePos;
            }
        }

        public void AddCreep(Creep creep) {
            ActiveCreeps.Add(creep);
        }

        public void AddTower(Tower tower) {
            ActiveTowers.Add(tower);
        }
        public void GetMousePos(object sender, InputStateEventArgs e) {
            MousePos = e.MousePos.ToVector2();
            MouseChanged = true;
        }
    }
}
