using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RunningfromCertainDeath.Screens
{
    public class OptionsMenu : MenuScreen
    {
        MenuStuff.MainMenuEntry controls, settings, plaques, back;
        Texture2D mouse;

        GameScreen prevScreen;

        string prev, next, selected, cancel;
        public override string PreviousEntryActionName
        {
            get { return prev; }
        }

        public override string NextEntryActionName
        {
            get { return next; }
        }

        public override string SelectedEntryActionName
        {
            get { return selected; }
        }

        public override string MenuCancelActionName
        {
            get { return cancel; }
        }

        //Constructor
        public OptionsMenu(GameScreen prevScn)
        {
            prevScreen = prevScn;
     
            // Set up action names
            prev = "MenuUp";
            next = "MenuDn";
            selected = "MenuSelect";
            cancel = "MenuExit";

            // Allow transitions
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // Text colors
            //selected = Highlighted = new Color(R, G, B);
            Selected = Color.Purple;
            Highlighted = Color.Aqua;
            Normal = Color.White;
        }

        public override void InitializeScreen()
        {
            EnableFade(Color.Black, 0.85f);
            
            // Reference 2 input system
            InputMap.NewAction(PreviousEntryActionName, Keys.Up);
            InputMap.NewAction(NextEntryActionName, Keys.Down);
            InputMap.NewAction(SelectedEntryActionName, Keys.Enter);
            InputMap.NewAction(SelectedEntryActionName, MousePresses.LeftMouse);
            InputMap.NewAction(MenuCancelActionName, Keys.Escape);

            // Init Entries
            settings = new MenuStuff.MainMenuEntry(this, "Settings", "Adjust game Settings");
            controls = new MenuStuff.MainMenuEntry(this, "Controls", "View game Controls/Keyboard layout");
            plaques = new MenuStuff.MainMenuEntry(this, "Plaques", "View all Plaques");
            back = new MenuStuff.MainMenuEntry(this, "Back", "Return to the Previous Menu");

            // Setup Screen events
            Removing += new EventHandler(OptionsMenuRemoving);
            Entering += new TransitionEventHandler(OptionsMenuScreen_Entering);
            Exiting += new TransitionEventHandler(OptionsMenuScreen_Exiting);

            // Setup Entry events, and load a submenu
            settings.Selected += new EventHandler(settings_Selected);
            controls.Selected += new EventHandler(controls_Selected);
            plaques.Selected += new EventHandler(plaques_Selected);
            back.Selected += new EventHandler(back_Selected);

            // Add entries to the list
            MenuEntries.Add(settings);
            MenuEntries.Add(controls);
            MenuEntries.Add(plaques);
            MenuEntries.Add(back);

            Viewport view = ScreenSystem.Viewport;
            SetDescriptionArea(new Rectangle(100, view.Height - 100, view.Width - 100, 50),
                new Color(11, 38, 40), new Color(29, 108, 117), new Point(10, 0), 0.5f);

            //AudioManager.singleton.PlaySong("Menu");
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            SpriteFont = content.Load<SpriteFont>(@"Fonts\menuFont");
            mouse = content.Load<Texture2D>(@"Images\mouseCursor");
            EnableMouse(mouse);

            //BackgroundTexture = content.Load<Texture2D>(@"Textures/Menu/MainMenuBG");

            settings.SetPosition(new Vector2(100, 200), true);
            controls.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), settings, true);
            plaques.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), controls, true);
            back.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), plaques, true);
           
            //string Title = "Options";
        }

        public override void UnloadContent()
        {
            SpriteFont = null;
        }

        void OptionsMenuScreen_Entering(object sender, TransitionEventArgs te)
        {
            //Slide effect from left to right
            float effect = (float)Math.Pow(te.percent - 1, 2) * -100;
            foreach (MenuEntry entry in MenuEntries)
            {
                entry.Acceleration = new Vector2(effect, 0);
                entry.Position = entry.InitialPosition + entry.Acceleration;
                entry.Opacity = te.percent;
            }
        }

        void OptionsMenuScreen_Exiting(object sender, TransitionEventArgs te)
        {
            // Slide effect from right to left
            float effect = (float)Math.Pow(te.percent - 1, 2) * 100;
            foreach (MenuEntry entry in MenuEntries)
            {
                entry.Acceleration = new Vector2(effect, 0);
                entry.Position = entry.InitialPosition - entry.Acceleration;
                entry.Scale = te.percent;
                entry.Opacity = te.percent;
            }
        }

        void settings_Selected(object sender, EventArgs e)
        {
            FreezeScreen();
            //ScreenSystem.AddScreen(new SettingsScreen());
        }

        void controls_Selected(object sender, EventArgs e)
        {
            FreezeScreen();
            ScreenSystem.AddScreen(new ControlsScreen(this));
        }

        void plaques_Selected(object sender, EventArgs e)
        {
            FreezeScreen();
            ScreenSystem.AddScreen(new PlaqueScreen(this));
        }

        void back_Selected(object sender, EventArgs e)
        {
            ExitScreen();
            prevScreen.ActivateScreen();
        }


        void OptionsMenuRemoving(object sender, EventArgs e)
        {
            MenuEntries.Clear();
        }
    }
}
