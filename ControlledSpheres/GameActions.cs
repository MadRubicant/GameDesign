using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ControlledSpheres {
    public delegate void GameAction(object[] Arguments);

    class GameContext {
        public Dictionary<AllButtons, GameAction> ContextKeybinds { get; set; }
        string ContextName; // Will probably replace with an enum

        public GameContext(string name) {
            ContextKeybinds = new Dictionary<AllButtons, GameAction>(); 
            ContextName = name;

        }
    }

    class InputLogic {
        List<GameContext> ContextList;

        public InputLogic() {

        }
    }

    
}
