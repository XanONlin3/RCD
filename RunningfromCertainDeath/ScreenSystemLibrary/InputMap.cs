using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ScreenSystemLibrary
{
    #region Triggers Enum
    /// <summary>
    /// Allows us to know when a trigger is used
    /// </summary>
    public enum Triggers
    {
        None,
        LeftTrigger,
        RightTrigger,
    }
    #endregion

    #region Mouse Enum
    /// <summary>
    /// Allows us to know when a mouse is used
    /// </summary>
    public enum MousePresses
    {
        None,
        LeftMouse,
        RightMouse,
    }
    #endregion
    public class InputMap
    {
        #region Input class
        /// <summary>
        /// Contains what any given input will have available
        /// </summary>
        public struct Input
        {
            Keys key;
            Buttons button;
            Triggers trigger;
            MousePresses mousepresses;

            public void setKey(Keys k)
            {
                key = k;
            }

            public Keys getKey()
            {
                return key;
            }

            public void setButton(Buttons b)
            {
                button = b;
            }

            public Buttons getButton()
            {
                return button;
            }

            public void setTrigger(Triggers t)
            {
                trigger = t;
            }

            public Triggers getTrigger()
            {
                return trigger;
            }

            public void setMousePress(MousePresses m)
            {
                mousepresses = m;
            }

            public MousePresses getMousePress()
            {
                return mousepresses;
            }
        }
        #endregion

        #region Key Bind
        //Dictionary to hold the list of inputs for any given command
        Dictionary<string, List<Input>> keybinds;
        #endregion

        #region Constructors
        public InputMap()
        {
            keybinds = new Dictionary<string, List<Input>>();
        }
        #endregion

        #region System Methods

        /// <summary>
        /// Adds or extends an action to the dictionary
        /// </summary>
        /// <param name="action">A given action that may or may not exist already</param>
        /// <param name="input">The input we want to give to this action</param>
        public void NewAction(string action, Input input)
        {
            if (keybinds.ContainsKey(action))
            {
                keybinds[action].Add(input);
            }
            else
                keybinds.Add(action, new List<Input>(new Input[] { input }));
        }

        public void NewAction(string action, Keys k)
        {
            Input input = new Input();
            input.setKey(k);
            NewAction(action, input);
        }

        public void NewAction(string action, Buttons b)
        {
            Input input = new Input();
            input.setButton(b);
            NewAction(action, input);
        }

        public void NewAction(string action, Triggers t)
        {
            if (t != Triggers.None)
            {
                Input input = new Input();
                input.setTrigger(t);
                NewAction(action, input);
            }
        }

        public void NewAction(string action, MousePresses m)
        {
            if (m != MousePresses.None)
            {
                Input input = new Input();
                input.setMousePress(m);
                NewAction(action, input);
            }
        }
        /// <summary>
        /// Get the keys associated with the given action
        /// </summary>
        /// <param name="actionname">The name of the action</param>
        /// <returns>The list of inputs from the given action</returns>
        public List<Input> GetKeybinds(string actionName)
        {
            return keybinds[actionName];
        }

        public Vector2 GetMousePosition()
        {
            return InputSystem.MousePosition();
        }


        public float GetTriggerValue(Triggers t)
        {
            if (t == Triggers.LeftTrigger)
            {
                return InputSystem.LeftTrigger();
            }
            else if (t == Triggers.RightTrigger)
            {
                return InputSystem.RightTrigger();
            }

            return 0;
        }

        public bool ActionPressed(string actionName)
        {
            List<Input> binds = keybinds[actionName];
            float triggerValue;
            foreach (Input i in binds)
            {
                triggerValue = GetTriggerValue(i.getTrigger());
                
                if (InputSystem.IsPressedKey(i.getKey()) || InputSystem.IsPressedButton(i.getButton()) 
                    || InputSystem.IsPressedMouse(i.getMousePress()) || triggerValue > InputSystem.triggerThreshold)
                {
                    return true;
                }
            }
            return false;
        }

        public bool NewActionPress(string actionName)
        {
            List<Input> binds = keybinds[actionName];
            float triggerValue;
            foreach (Input i in binds)
            {
                triggerValue = GetTriggerValue(i.getTrigger());

                if (InputSystem.IsNewKeyPress(i.getKey()) || InputSystem.IsNewButtonPress(i.getButton())
                    || InputSystem.IsNewMousePress(i.getMousePress()) || triggerValue > InputSystem.triggerThreshold)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HeldAction(string actionName)
        {
            List<Input> binds = keybinds[actionName];
            float triggerValue;
            foreach (Input i in binds)
            {
                triggerValue = GetTriggerValue(i.getTrigger());
                if (InputSystem.IsHeldKey(i.getKey()) || InputSystem.IsHeldButton(i.getButton())
                    || InputSystem.IsHeldMousePress(i.getMousePress()) || triggerValue > InputSystem.triggerThreshold)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}