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
    public class MainMenuScreen : MenuScreen
    {
        MenuStuff.MainMenuEntry play, options, how2play, credits, quit;
        Texture2D mouse;

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
        public MainMenuScreen()
        {
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
            // Reference 2 input system
            InputMap.NewAction(PreviousEntryActionName, Keys.Up);
            InputMap.NewAction(NextEntryActionName, Keys.Down);
            InputMap.NewAction(SelectedEntryActionName, Keys.Enter);
            InputMap.NewAction(SelectedEntryActionName, MousePresses.LeftMouse);
            InputMap.NewAction(MenuCancelActionName, Keys.Escape);

            // Init Entries
            play = new MenuStuff.MainMenuEntry(this, "PLAY", "PLAY THE GAME");
            options = new MenuStuff.MainMenuEntry(this, "Options", "View Controls, change game Setting, and view Plaques"); // Plaques, etc..
            how2play = new MenuStuff.MainMenuEntry(this, "How-to", "General game info and help tutorial");
            credits = new MenuStuff.MainMenuEntry(this, "Credits", "GMs, Devs, and Special Thanks");
            quit = new MenuStuff.MainMenuEntry(this, "Quit", "Exit the game");

            // Setup Screen events
            Removing += new EventHandler(MainMenuRemoving);
            Entering += new TransitionEventHandler(MainMenuScreen_Entering);
            Exiting += new TransitionEventHandler(MainMenuScreen_Exiting);

            // Setup Entry events, and load a submenu
            play.Selected += new EventHandler(Play_Selected);
            options.Selected += new EventHandler(options_Selected);
            how2play.Selected += new EventHandler(how2play_Selected);
            credits.Selected += new EventHandler(Credit_Selected);
            quit.Selected += new EventHandler(Quit_Selected);

            // Add entries to the list
            MenuEntries.Add(play);
            MenuEntries.Add(options);
            MenuEntries.Add(how2play);
            MenuEntries.Add(credits);
            MenuEntries.Add(quit);

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
            
            BackgroundTexture = content.Load<Texture2D>(@"Textures/Menu/mainMenuBG");

            play.SetPosition(new Vector2(100, 200), true);
            options.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), play, true);
            how2play.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), options, true);
            credits.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), how2play, true);
            quit.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), credits, true);

            //string Title = "Running from Certain Death\n";
        }

        public override void UnloadContent()
        {
            SpriteFont = null;
        }

        void MainMenuScreen_Entering(object sender, TransitionEventArgs te)
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

        void MainMenuScreen_Exiting(object sender, TransitionEventArgs te)
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

        void Play_Selected(object sender, EventArgs e)
        {
            ExitScreen();
            ScreenSystem.AddScreen(new PlayScreen());
            //ScreenSystem.AddScreen(new LevelSelectionScreen());
        }

        void how2play_Selected(object sender, EventArgs e)
        {
            FreezeScreen();
            ScreenSystem.AddScreen(new How2PlayScreen(this));
        }

        void options_Selected(object sender, EventArgs e)
        {
            FreezeScreen();
            ScreenSystem.AddScreen(new OptionsMenu(this));
        }

        void Credit_Selected(object sender, EventArgs e)
        {
            FreezeScreen();
            ScreenSystem.AddScreen(new CreditScreen(this));
        }

        void Quit_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }


        void MainMenuRemoving(object sender, EventArgs e)
        {
            MenuEntries.Clear();
        }
    }
}
