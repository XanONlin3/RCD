using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ScreenSystemLibrary
{
    public class InputSystem
    {
        #region State Data

        // CurrentKeyboardState will have public get and private set
        public static KeyboardState CurrentKeyboardState
        {
            get;
            private set;
        }

        // PreviousKeyboardState will have public get and private set
        public static KeyboardState PreviousKeyboardState
        {
            get;
            private set;
        }

        // CurrentGamepadState will have public get and private set
        public static GamePadState CurrentGamepadState
        {
            get;
            private set;
        }

        // PreviousGamepadState will have public get and private set
        public static GamePadState PreviousGamepadState
        {
            get;
            private set;
        }

        public static MouseState CurrentMouseState
        {
            get;
            private set;
        }

        public static MouseState PreviousMouseState
        {
            get;
            private set;
        }
        #endregion

        #region State Methods
        /// <summary>
        /// Check to see if a key is pressed.
        /// </summary>
        /// <param name="k">The key we want to check</param>
        /// <returns>True if the provided key is pressed</returns>
        public static bool IsPressedKey(Keys k)
        {
            return CurrentKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Check to see if a key is newly pressed.
        /// </summary>
        /// <param name="k">The key we want to check</param>
        /// <returns>True if the provided key is a new press (pressed currently
        /// but not pressed before).</returns>
        public static bool IsNewKeyPress(Keys k)
        {
            return CurrentKeyboardState.IsKeyDown(k)
                && PreviousKeyboardState.IsKeyUp(k);
        }

        /// <summary>
        /// Check to see if a key is held.
        /// </summary>
        /// <param name="k">The key we want to check</param>
        /// <returns>True if the key is pressed currently and previously</returns>
        public static bool IsHeldKey(Keys k)
        {
            return CurrentKeyboardState.IsKeyDown(k)
                && PreviousKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Check to see if a button is pressed.
        /// </summary>
        /// <param name="b">The button we want to check</param>
        /// <returns>True if the provided button is pressed</returns>
        public static bool IsPressedButton(Buttons b)
        {
            if ((int)b == 0) return false;
            return CurrentGamepadState.IsButtonDown(b);

        }

        /// <summary>
        /// Check to see if a button is newly pressed.
        /// </summary>
        /// <param name="b">The button we want to check</param>
        /// <returns>True if the provided button is a new press (pressed currently
        /// but not pressed before).</returns>
        public static bool IsNewButtonPress(Buttons b)
        {
            return CurrentGamepadState.IsButtonDown(b)
                && PreviousGamepadState.IsButtonUp(b);
        }

        /// <summary>
        /// Check to see if a button is held.
        /// </summary>
        /// <param name="b">The button we want to check</param>
        /// <returns>True if the button is pressed currently and previously</returns>
        public static bool IsHeldButton(Buttons b)
        {
            return CurrentGamepadState.IsButtonDown(b)
                && PreviousGamepadState.IsButtonDown(b);
        }

        public static bool IsPressedMouse(MousePresses m)
        {
            if (m == MousePresses.LeftMouse)
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed;
            }
            else if (m == MousePresses.RightMouse)
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed;
            }

            return false;
        }

        public static bool IsNewMousePress(MousePresses m)
        {
            if (m == MousePresses.LeftMouse)
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    PreviousMouseState.LeftButton == ButtonState.Released;
            }
            else if(m == MousePresses.RightMouse)
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed &&
                    PreviousMouseState.RightButton == ButtonState.Released;
            }

            return false;
        }

        public static bool IsHeldMousePress(MousePresses m)
        {
            if (m == MousePresses.LeftMouse)
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    PreviousMouseState.LeftButton == ButtonState.Pressed;
            }
            else if (m == MousePresses.RightMouse)
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed && 
                    PreviousMouseState.RightButton == ButtonState.Pressed;
            }

            return false;
        }

        public const float triggerThreshold = 0.2f;
        public static float LeftTrigger() { return CurrentGamepadState.Triggers.Left; }
        public static float RightTrigger() { return CurrentGamepadState.Triggers.Right; }
        public static Vector2 MousePosition() { return new Vector2(CurrentMouseState.X, CurrentMouseState.Y); }
        #endregion

        public InputSystem()
        {
            CurrentGamepadState = new GamePadState();
            PreviousGamepadState = new GamePadState();
            CurrentKeyboardState = new KeyboardState();
            PreviousKeyboardState = new KeyboardState();
            CurrentMouseState = new MouseState();
            PreviousMouseState = new MouseState();
        }

        public static void Update(GameTime gameTime)
        {
            PreviousGamepadState = CurrentGamepadState;
            CurrentGamepadState = GamePad.GetState(PlayerIndex.One);

            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }
    }
}
