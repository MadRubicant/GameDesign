using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    // This entire class is a hack to get things working
    class EntityManager {
        HashSet<Tower> ActiveTowers;
        List<Tower> TowersToRemove;
        // The value for this dict is the current waypoint the creep is heading towards
        Dictionary<Creep, int> CreepWaypointDict;
        List<Creep> CreepsToRemove;

        HashSet<Projectile> ActiveProjectiles;
        List<Projectile> ProjectilesToRemove;

        public CreepPath creepPath;
        bool MouseChanged = false;
        Vector2 MousePos;
        public EntityManager() {
            ActiveTowers = new HashSet<Tower>();
            TowersToRemove = new List<Tower>();
            CreepWaypointDict = new Dictionary<Creep, int>();
            CreepsToRemove = new List<Creep>();
            ActiveProjectiles = new HashSet<Projectile>();
            ProjectilesToRemove = new List<Projectile>();
        }

        public EntityManager(Vector2[] WaypointArray) {
            ActiveTowers = new HashSet<Tower>();
            CreepWaypointDict = new Dictionary<Creep, int>();
            ActiveProjectiles = new HashSet<Projectile>();
            TowersToRemove = new List<Tower>(100);
            CreepsToRemove = new List<Creep>(100);
            ProjectilesToRemove = new List<Projectile>(100);
            creepPath = new CreepPath(WaypointArray);
        }

        public void Update(GameTime gameTime) {
            List<Creep> DirtyCreeps = new List<Creep>();
            foreach (var v in CreepWaypointDict) {
                Creep c = v.Key;
                if (creepPath.UpdateCreep(c, v.Value) == true)
                    DirtyCreeps.Add(c);
                c.Update(gameTime);
                //c.Rotation = c.Center - MousePos;
            }

            foreach (Tower t in ActiveTowers) {
                t.Update(gameTime);
                t.Rotation = t.Center - MousePos;
            }

            foreach (Creep c in DirtyCreeps)
                CreepWaypointDict[c]++;

            foreach (Projectile p in ActiveProjectiles) {
                p.Update(gameTime);
            }

            CollisionDetection();
            foreach (Tower T in TowersToRemove)
                ActiveTowers.Remove(T);
            foreach (Creep C in CreepsToRemove)
                CreepWaypointDict.Remove(C);
            foreach (Projectile P in ProjectilesToRemove)
                ActiveProjectiles.Remove(P);
            TowersToRemove.Clear();
            CreepsToRemove.Clear();
            ProjectilesToRemove.Clear();
        }

        public void Draw(SpriteBatch spritebatch) {
            foreach (Tower T in ActiveTowers)
                T.Draw(spritebatch);
            foreach (Creep C in CreepWaypointDict.Keys)
                C.Draw(spritebatch);
            foreach (Projectile P in ActiveProjectiles)
                P.Draw(spritebatch);
        }

        public void AddCreep(Creep creep) {
            creep.Center = creepPath.Beginning();
            CreepWaypointDict.Add(creep, 1);
        }

        public void AddTower(Tower tower) {
            ActiveTowers.Add(tower);
        }

        public void AddProjectile(Projectile projectile) {
            if (projectile == null)
                return;
            ActiveProjectiles.Add(projectile);
        }

        public void GetMousePos(object sender, InputStateEventArgs e) {
            MousePos = e.MousePos.ToVector2();
            MouseChanged = true;
        }

        private void CollisionDetection () {
            // Projectiles can only collide with creeps; 
            foreach (Projectile P in ActiveProjectiles) {
                if (P.CollisionActive == true) {
                    foreach (Creep C in CreepWaypointDict.Keys) {
                        if (P.Hitbox.Intersects(C.Hitbox)) {
                            C.HP -= P.Damage;
                            ProjectilesToRemove.Add(P);
                        }
                    }
                }
                else {
                    if (P.Hitbox.Intersects(P.Target.Hitbox)) {
                        Creep C = P.Target as Creep;
                        if (C != null) {
                            C.HP -= P.Damage;
                            ProjectilesToRemove.Add(P);
                        }
                    }
                }
            }
        }
    }
}
