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
    public class PauseScreen : MenuScreen
    {
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

        MenuStuff.MainMenuEntry resume, options, how2play, quit;
        Texture2D mouse;

        GameScreen prevScreen;
        //Session session;
       
        //Constructor
        public PauseScreen(GameScreen prevScr) //Session s
        {
            // Reference parent to resume when pause screen is done
            prevScreen = prevScr;
            
            // Set up action names
            prev = "MenuUp";
            next = "MenuDn";
            selected = "MenuSelect";
            cancel = "MenuExit";

            // Text colors
            Selected = Color.Orange;
            Highlighted = Color.Aqua;
            Normal = Color.White;

            //sessions = s;
        }

        public override void InitializeScreen()
        {
            //Fade the screen below
            EnableFade(Color.Black, 0.8f);

            InputMap.NewAction(PreviousEntryActionName, Keys.Up);
            InputMap.NewAction(NextEntryActionName, Keys.Down);
            InputMap.NewAction(SelectedEntryActionName, Keys.Enter);
            InputMap.NewAction(SelectedEntryActionName, MousePresses.LeftMouse);
            InputMap.NewAction(MenuCancelActionName, Keys.Escape);

            //Init the entries and set up the events
            resume = new MenuStuff.MainMenuEntry(this, "Resume", "Continue game");
            resume.Selected += new EventHandler(ResumeSelect);

            options = new MenuStuff.MainMenuEntry(this, "Options", "Game Options");
            options.Selected += new EventHandler(OptionsSelect);

            how2play = new MenuStuff.MainMenuEntry(this, "Help", "Game controls, and general game tips");
            how2play.Selected += new EventHandler(how2playSelect);

            quit = new MenuStuff.MainMenuEntry(this, "Quit", "Quit game... Loser");
            quit.Selected += new EventHandler(QuitSelect);

            // Setup Screen events
            Removing += new EventHandler(PauseMenuRemoving);
            Entering += new TransitionEventHandler(PauseMenuScreen_Entering);
            Removing += new EventHandler(PauseMenuRemoving);

            //Add all entries to the list
            MenuEntries.Add(resume);
            MenuEntries.Add(options);
            MenuEntries.Add(how2play);
            MenuEntries.Add(quit);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            SpriteFont = content.Load<SpriteFont>(@"fonts\menuFont");
            mouse = content.Load<Texture2D>(@"Images\mouseCursor");
            EnableMouse(mouse);

            //Set up positioning
            resume.SetPosition(new Vector2(100, 200), true);
            options.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), resume, true);
            how2play.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), options, true);
            quit.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), how2play, true);
        }

        void PauseMenuScreen_Entering(object sender, TransitionEventArgs te)
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

        void PauseMenuScreen_Exiting(object sender, TransitionEventArgs te)
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

        void ResumeSelect(object sender, EventArgs e)
        {
            ExitScreen();
        }

        void how2playSelect(object sender, EventArgs e)
        {
            FreezeScreen();
            ScreenSystem.AddScreen(new How2PlayScreen(this));

        }

        void OptionsSelect(object sender, EventArgs e)
        {
            FreezeScreen();
            ScreenSystem.AddScreen(new OptionsMenu(this));
        }

        void QuitSelect(object sender, EventArgs e)
        {
            ScreenSystem.Game.Exit();
        }

        void PauseMenuRemoving(object sender, EventArgs e)
        {
            MenuEntries.Clear();
            prevScreen.ActivateScreen();
            //session.Resume();
        }
    }
}
