using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ControlledSpheres {

    public delegate void InputEvent(object sender, InputStateEventArgs e);

    enum ActionTypes { Action, State, Range };

    /// <summary>
    /// Enumeration of buttons that can be used for input. Includes keyboard keys, mouse buttons, and will be extended to include gamepad buttons
    /// </summary>
    public enum AllButtons { None,
        Q, W, E, R, T, Y, U, I, O, P, A, S, D, F, G, H, J, K, L, Z, X, C, V, B, N, M, // Letter keys
        Dash, Equal, OpenBracket, CloseBracket, Backslash, Semicolon, Apostrophe, Comma, Period, Slash, Plus,// Non-numeric keys
        Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, Num0, Numpad1, Numpad2, Numpad3, Numpad4, Numpad5, // Number and numpad keys
        Numpad6, Numpad7, Numpad8, Numpad9, Numpad0, NumpadSlash, NumpadAsterisk, NumpadDash, NumpadPlus, NumpadPeriod,
        Escape, Tab, Enter, Backspace, Delete, LeftSingleQuote, Spacebar, // Misc keys
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, // F-keys
        MouseButtonLeft, MouseButtonMiddle, MouseButtonRight, // Mouse
        ArrowDown, ArrowLeft, ArrowUp, ArrowRight // Arrowkeys
    }

    [Flags]
    public enum ModifierKeys { None = 0, Left = 1, Right = 2, Control = 4, Alt = 8, Shift = 16, LeftAlt = Left | Alt, RightAlt = Right | Alt,
        LeftControl = Left | Control, RightControl = Right | Control, LeftShift = Left | Shift, RightShift = Right | Shift,
    }

    public class InputStateEventArgs : EventArgs {
        public AllButtons Button { get; set; }
        public ModifierKeys Modifier { get; set; }
        public Point MousePos {get; set;}
        public Point MouseDelta { get; set; }

        public InputStateEventArgs Copy() {
            var ret = new InputStateEventArgs();
            ret.Button = this.Button;
            ret.Modifier = this.Modifier;
            ret.MousePos = this.MousePos;
            return ret;
        }
    }


    public class InputHandler {
        // ButtonPressed, ButtonHeld, and ButtonReleased are events for when a keyboard key, mouse button, or gamepad button are 
        // pressed, held, or released

        // ButtonPressed triggers when a button is pressed in the current frame, but wasn't in the previous
        public event InputEvent ButtonPressed;

        // ButtonHeld triggers when a button is currently being pressed, doesn't care about the button history
        public event InputEvent ButtonHeld;

        // ButtonReleased triggers when a button is released in the current frame, but was pressed previously
        public event InputEvent ButtonReleased;

        // MouseMovement... idk, I'll figure it out
        public event InputEvent MouseMovement;

        public Vector2 MousePos {
            get {
                return this.MouseCurrent.Position.ToVector2();
            }
        }
        KeyboardState KeyboardCurrent, KeyboardPrevious;
        MouseState MouseCurrent, MousePrevious;
        GamePadState GamepadCurrent, GamepadPrevious;
        Keys[] PreviousKeyState, CurrentKeyState;

        public InputHandler() {
            KeyboardPrevious = KeyboardCurrent = new KeyboardState();
            MouseCurrent = MousePrevious = new MouseState();
            GamepadCurrent = GamepadPrevious = new GamePadState();
            PreviousKeyState = CurrentKeyState = new Keys[0];
        }

        public void HandleInput() {
            // Get the status of input devices for the current frame, and store the values from last frame
            KeyboardPrevious = KeyboardCurrent;
            MousePrevious = MouseCurrent;
            GamepadPrevious = GamepadCurrent;
            KeyboardCurrent = Keyboard.GetState();
            MouseCurrent = Mouse.GetState();
            GamepadCurrent = GamePad.GetState(PlayerIndex.One);

            PreviousKeyState = CurrentKeyState;
            CurrentKeyState = KeyboardCurrent.GetPressedKeys();
            // All keys K are currently pressed in this frame
            // TODO make this actually send modifiers and keys at the same time
            InputStateEventArgs args = new InputStateEventArgs();

            ModifierKeys ModKey = ModifierKeys.None;
            foreach (Keys K in CurrentKeyState) {
                ModifierKeys MK = mapKeytoModifier(K);
                if (MK != ModifierKeys.None) {
                    ModKey |= MK;
                }
            }
            args.Modifier = ModKey;
            args.MousePos = MouseCurrent.Position;

            #region Keyboard inputs
            foreach (Keys K in CurrentKeyState) {
                args.Button = mapKeytoButton(K);
                if (KeyboardPrevious.IsKeyUp(K)) {
                    if (ButtonPressed != null) {
                        ButtonPressed(this, args);
                    }
                }
                if (KeyboardPrevious.IsKeyDown(K)) {
                    if (ButtonHeld != null) {
                        ButtonHeld(this, args);
                    }
                }
            }

            // This gets all the ButtonReleased events
            foreach (Keys K in PreviousKeyState) {
                if (KeyboardCurrent.IsKeyUp(K)) {
                    args.Button = mapKeytoButton(K);
                    if (ButtonReleased != null) {
                        ButtonReleased(this, args);
                    }
                }
            }
            #endregion

            #region Mouse inputs
            args.Button = AllButtons.MouseButtonLeft;
            if (MouseCurrent.LeftButton == ButtonState.Pressed && MousePrevious.LeftButton == ButtonState.Released) {
                if (ButtonPressed != null)
                    ButtonPressed(this, args);
            }
            else if (MouseCurrent.LeftButton == ButtonState.Pressed) {
                if (ButtonHeld != null) 
                    ButtonHeld(this, args);
            }
            else if (MouseCurrent.LeftButton == ButtonState.Released && MousePrevious.LeftButton == ButtonState.Pressed) {
                if (ButtonReleased != null) 
                    ButtonReleased(this, args);
            }

            args.Button = AllButtons.MouseButtonMiddle;
            if (MouseCurrent.MiddleButton == ButtonState.Pressed && MousePrevious.MiddleButton == ButtonState.Released) {
                if (ButtonPressed != null)
                    ButtonPressed(this, args);
            }
            else if (MouseCurrent.MiddleButton == ButtonState.Pressed) {
                if (ButtonHeld != null) 
                    ButtonHeld(this, args);
            }
            else if (MouseCurrent.MiddleButton == ButtonState.Released && MousePrevious.MiddleButton == ButtonState.Pressed) {
                if (ButtonReleased != null)
                    ButtonReleased(this, args);
            }

            args.Button = AllButtons.MouseButtonRight;
            if (MouseCurrent.RightButton == ButtonState.Pressed && MousePrevious.RightButton == ButtonState.Released) {
                if (ButtonPressed != null)
                    ButtonPressed(this, args);
            }
            else if (MouseCurrent.RightButton == ButtonState.Pressed) {
                if (ButtonHeld != null)
                    ButtonHeld(this, args);
            }
            else if (MouseCurrent.RightButton == ButtonState.Released && MousePrevious.RightButton == ButtonState.Pressed)
                if (ButtonReleased != null)
                    ButtonReleased(this, args);
            // I feel dirty
            #endregion

            #region Mouse Movement
            Point mouseDiff = (MouseCurrent.Position - MousePrevious.Position);
            if (mouseDiff != new Point()) {
                args.MouseDelta = mouseDiff;
                if (MouseMovement != null)
                    MouseMovement(this, args);
            }

            #endregion
            
        }

        // God this function is ugly
        // It's literally a switch statement with 85 cases
        // At least it's alphanumerically sorted. Mostly
        private AllButtons mapKeytoButton(Keys K) {
            switch (K) {
                case Keys.A:
                    return AllButtons.A;
                case Keys.Add:
                    return AllButtons.NumpadPlus;
                case Keys.B:
                    return AllButtons.B;
                case Keys.Back:
                    return AllButtons.Backspace;
                case Keys.C:
                    return AllButtons.C;
                case Keys.D:
                    return AllButtons.D;
                case Keys.D1:
                    return AllButtons.Num1;
                case Keys.D2:
                    return AllButtons.Num2;
                case Keys.D3:
                    return AllButtons.Num3;
                case Keys.D4:
                    return  AllButtons.Num4;
                case Keys.D5:
                    return AllButtons.Num5;
                case Keys.D6:
                    return AllButtons.Num6;
                case Keys.D7:
                    return AllButtons.Num7;
                case Keys.D8:
                    return AllButtons.Num8;
                case Keys.D9:
                    return AllButtons.Num9;
                case Keys.D0:
                    return AllButtons.Num0;
                case Keys.Decimal:
                    return AllButtons.NumpadPeriod;
                case Keys.Delete:
                    return AllButtons.Delete;
                case Keys.Divide:
                    return AllButtons.NumpadSlash;
                case Keys.Down:
                    return AllButtons.ArrowUp;
                case Keys.E:
                    return AllButtons.E;
                case Keys.Enter:
                    return AllButtons.Enter;
                case Keys.Escape:
                    return AllButtons.Escape;
               case Keys.F:
                    return AllButtons.F;
                case Keys.F1:
                    return AllButtons.F1;
                case Keys.F2:
                    return AllButtons.F2;
                case Keys.F3:
                    return AllButtons.F3;
                case Keys.F4:
                    return AllButtons.F4;
                case Keys.F5:
                    return AllButtons.F5;
                case Keys.F6:
                    return AllButtons.F6;
                case Keys.F7:
                    return AllButtons.F7;
                case Keys.F8:
                    return AllButtons.F8;
                case Keys.F9:
                    return AllButtons.F9;
                case Keys.F10:
                    return AllButtons.F10;
                case Keys.F11:
                    return AllButtons.F11;
                case Keys.F12:
                    return AllButtons.F12;
                case Keys.G:
                    return AllButtons.G;
                case Keys.H:
                    return AllButtons.H;
               case Keys.I:
                    return AllButtons.I;
                case Keys.J:
                    return AllButtons.J;
                case Keys.K:
                    return AllButtons.K;
                case Keys.L:
                    return AllButtons.L;
                case Keys.Left:
                    return AllButtons.ArrowLeft;
                case Keys.M:
                    return AllButtons.M;
                case Keys.Multiply:
                    return AllButtons.NumpadAsterisk;
                case Keys.N:
                    return AllButtons.N;
                case Keys.NumPad0:
                    return AllButtons.Numpad0;
                case Keys.NumPad1:
                    return AllButtons.Numpad1;
                case Keys.NumPad2:
                    return AllButtons.Numpad2;
                case Keys.NumPad3:
                    return AllButtons.Numpad3;
                case Keys.NumPad4:
                    return AllButtons.Numpad4;
                case Keys.NumPad5:
                    return AllButtons.Numpad5;
                case Keys.NumPad6:
                    return AllButtons.Numpad6;
                case Keys.NumPad7:
                    return AllButtons.Numpad7;
                case Keys.NumPad8:
                    return AllButtons.Numpad8;
                case Keys.NumPad9:
                    return AllButtons.Numpad9;
                case Keys.O:
                    return AllButtons.O;
                case Keys.OemBackslash:
                    return AllButtons.Backslash;
                case Keys.OemCloseBrackets:
                    return AllButtons.CloseBracket;
                case Keys.OemComma:
                    return AllButtons.Comma;
                case Keys.OemMinus:
                    return AllButtons.Dash;
                case Keys.OemOpenBrackets:
                    return AllButtons.OpenBracket;
                case Keys.OemPeriod:
                    return AllButtons.Period;
                case Keys.OemPlus:
                    return AllButtons.Plus;
                case Keys.OemQuestion:
                    return AllButtons.Slash;
                case Keys.OemQuotes:
                    return AllButtons.Apostrophe;
                case Keys.OemSemicolon:
                    return AllButtons.Semicolon;
                case Keys.OemTilde:
                    return AllButtons.LeftSingleQuote;
                case Keys.P:
                    return AllButtons.P;
                case Keys.Q:
                    return AllButtons.Q;
                case Keys.R:
                    return AllButtons.R;
                case Keys.Right:
                    return AllButtons.ArrowRight;
                case Keys.S:
                    return AllButtons.S;
                case Keys.Space:
                    return AllButtons.Spacebar;
                case Keys.Subtract:
                    return AllButtons.NumpadDash;
                case Keys.T:
                    return AllButtons.T;
                case Keys.Tab:
                    return AllButtons.Tab;
                case Keys.U:
                    return AllButtons.U;
                case Keys.Up:
                    return AllButtons.ArrowUp;
                case Keys.V:
                    return AllButtons.V;
                case Keys.W:
                    return AllButtons.W;
                case Keys.X:
                    return AllButtons.X;
                case Keys.Y:
                    return AllButtons.Y;
                case Keys.Z:
                    return AllButtons.Z;
                default:
                    return AllButtons.None;
            }
        }

        // This one only has 6 cases
        private ModifierKeys mapKeytoModifier(Keys K) {
            switch (K) {
                case Keys.LeftAlt:
                    return ModifierKeys.LeftAlt;
                case Keys.LeftControl:
                    return ModifierKeys.LeftControl;
                case Keys.LeftShift:
                    return ModifierKeys.LeftShift;
                case Keys.RightAlt:
                    return ModifierKeys.RightAlt;
                case Keys.RightControl:
                    return ModifierKeys.RightControl;
                case Keys.RightShift:
                    return ModifierKeys.RightShift;
                default:
                    return ModifierKeys.None;
            }
        }
    }
}
