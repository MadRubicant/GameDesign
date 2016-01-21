using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ControlledSpheres {

    delegate void InputCallback(object sender, InputStateEventArgs e);

    enum ActionTypes {Action, State, Range};
    enum MouseButtons {LeftButton, RightButton, MiddleButton };

    class Interface {
        public Dictionary<Keys, ActionTypes> KeybindTypes;
        public Dictionary<MouseButtons, ActionTypes> MouseBindTypes;

        KeyboardState previousKeyState, currentKeyState;
        MouseState previousMouseState, currentMouseState;

        public event InputCallback ButtonPressed;
        public event InputCallback ButtonReleased;
        public event InputCallback StateActive;

        public Interface() {

        }

        public void HandleInput(KeyboardState keystate, MouseState mousestate) {
            // Switch out old values
            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;
            currentKeyState = keystate;
            currentMouseState = mousestate;

            foreach (Keys key in Enum.GetValues(typeof(Keys))) {
                if (currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key)) {
                    if (KeybindTypes[key] == ActionTypes.Action) {
                        InputStateEventArgs e = new InputStateEventArgs();
                        e.KState = currentKeyState;
                        e.MState = currentMouseState;
                        if (ButtonPressed != null) {
                            ButtonPressed(this, e);
                        }
                        continue;
                    }
                }

                if (currentKeyState.IsKeyDown(key)) {
                    if (KeybindTypes[key] == ActionTypes.State) {
                        InputStateEventArgs e = new InputStateEventArgs();
                        e.KState = currentKeyState;
                        e.MState = currentMouseState;
                        if (StateActive != null)
                            StateActive(this, e);
                        continue;
                    }
                }

                if (!currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyDown(key)) {
                    InputStateEventArgs e = new InputStateEventArgs();
                    e.KState = currentKeyState;
                    e.MState = currentMouseState;
                    if (ButtonReleased != null)
                        ButtonReleased(this, e);
                    continue;
                }
            }
        }
    }

    class InputStateEventArgs : EventArgs {
        public KeyboardState KState { get; set; }
        public MouseState MState { get; set; }
    }

}
