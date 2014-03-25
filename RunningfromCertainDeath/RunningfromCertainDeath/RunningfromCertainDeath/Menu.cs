using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RunningfromCertainDeath
{
    class Menu
    {
        private bool init = false;
        private List <MenuItem> menuItems {get; set;}
        public int Count
        {
            get { return menuItems.Count; }
        }

        public string Title { get; set; }       //heading of menu
        public string InfoTxt { get; set; }     //sub info
        public int lastNavigated { get; set; }  //stores last gametime the menu was navigated (for response time)

        /* Logic of selecting menu Items:
         * Private field stores the index of selected item and
         * public field makes it accessible
         */

        private int _selectedIndex;
        public int selectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            protected set
            {
                if (value >= menuItems.Count || value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _selectedIndex = value;
            }
        }

        public MenuItem SelectedItem
        {
            get
            {
                return menuItems[selectedIndex];
            }
        }

        // Constructors
        public Menu(string title)
        {
            menuItems = new List<MenuItem>();
            Title = title;
            InfoTxt = "";
        }

        public Menu(string title, string infotxt)
        {
            menuItems = new List<MenuItem>();
            Title = title;
            InfoTxt = infotxt;
        }

        // Adding Items to the list (virtual so they are overwritten in child class)
        public virtual void AddMenuItem(string title, Action<Buttons> action)
        {
            AddMenuItem(title, action, "");
        }
        // Overload
        public virtual void AddMenuItem(string title, Action<Buttons> action, string discription)
        {
            menuItems.Add(new MenuItem {Title = title, Description =discription, Action =action});
            selectedIndex = 0; 
        }

        // Draw Menu
        public void DrawMenu(SpriteBatch spriteBatch, int screenWidth, SpriteFont font, int yPos, Vector2 descriptionPos, Color itemColor, Color selectedColor)
        {   
            //Centered vector: screenWidth / 2 - font.MeasureString(title).X / 2, yPos)
            spriteBatch.DrawString(font, Title, new Vector2(250, yPos), Color.White);
            yPos += (int)font.MeasureString(Title).Y + 10;
            for(int i =0; i < Count; i++)
            {
                Color color = itemColor;
                if (i == selectedIndex)
                {
                    color = selectedColor;
                }
                spriteBatch.DrawString(font, menuItems[i].Title, new Vector2(250, yPos), color);
                yPos += (int)font.MeasureString(menuItems[i].Title).Y + 10;
            }
            spriteBatch.DrawString(font, menuItems[selectedIndex].Description, descriptionPos, selectedColor);
        
        }

        // Navigation
        public void Navigate(KeyboardState keyState, GameTime gameTime)
        {
            if (!init)
            {
                lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
                init = true;
            }

            keyState =Keyboard.GetState();
            if (gameTime.TotalGameTime.TotalMilliseconds - lastNavigated > 250)
            {
                if (keyState.IsKeyDown(Keys.Down) && selectedIndex < Count -1)
                {
                    selectedIndex++;
                    lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }
                if (keyState.IsKeyDown(Keys.Up) && selectedIndex > 0)
                {
                    selectedIndex--;
                    lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }

                // IF we use GAMEPAD
             /* if (GamePadState.Buttons.A == ButtonState.Pressed)
                {
                    selectedItem.Action(Buttons.X);
                    lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
                } */
            }
        }



    }//class
}//nameSpace
