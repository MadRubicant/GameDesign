using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ControlledSpheres {
    class EntityManager {
        HashSet<Tower> ActiveTowers;

        // The value for this dict is the current waypoint the creep is heading towards
        Dictionary<Creep, int> CreepWaypointDict;

        public CreepPath creepPath;
        bool MouseChanged = false;
        Vector2 MousePos;
        public EntityManager() {
            ActiveTowers = new HashSet<Tower>();
            CreepWaypointDict = new Dictionary<Creep, int>();
        }

        public EntityManager(Vector2[] WaypointArray) {
            ActiveTowers = new HashSet<Tower>();
            CreepWaypointDict = new Dictionary<Creep, int>();
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
            foreach (Creep c in DirtyCreeps)
                CreepWaypointDict[c]++;


            foreach (Tower t in ActiveTowers) {
                t.Update(gameTime);
                t.Rotation = t.Center - MousePos;
            }
        }

        public void AddCreep(Creep creep) {
            creep.Center = creepPath.Beginning();
            CreepWaypointDict.Add(creep, 1);
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
