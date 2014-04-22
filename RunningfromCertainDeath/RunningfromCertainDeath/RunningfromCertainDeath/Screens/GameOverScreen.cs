using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace RunningfromCertainDeath.Screens
{
    public class GameOverScreen : MenuScreen
    {
        MenuStuff.MainMenuEntry play, quit;
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

        LevelSelectionScreen levelselect;

        public GameOverScreen(LevelSelectionScreen lvSelect)
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
            //selected = Highlighted = Color.(R, G, B);
            Selected = Color.Purple;
            Highlighted = Color.Aqua;
            Normal = Color.White;

            levelselect = lvSelect;
        }
        public override void InitializeScreen()
        {
            // Reference 2 input system
            InputMap.NewAction(PreviousEntryActionName, Keys.Up);
            InputMap.NewAction(NextEntryActionName, Keys.Down);
            InputMap.NewAction(SelectedEntryActionName, Keys.Enter);
            InputMap.NewAction(SelectedEntryActionName, MousePresses.LeftMouse);
            InputMap.NewAction(MenuCancelActionName, Keys.Escape);

            play =new MenuStuff.MainMenuEntry (this, "Play Again?", "GAME OVER - Soooo, just got your butt kicked? Come n' get it kicked somemore!!");
            quit = new MenuStuff.MainMenuEntry(this, "Quit", "Soooo, you finished gettin' your butt kicked... LOSER!");

            // Setup Screen events
            Removing += new EventHandler(GameOverScreen_Removing);
            Entering += new TransitionEventHandler(GameOverScreen_Entering);
            Exiting += new TransitionEventHandler(GameOverScreen_Exiting);

            play.Selected += new EventHandler(play_Selected);
            quit.Selected += new EventHandler(quit_Selected);

            MenuEntries.Add(play);
            MenuEntries.Add(quit);

            Viewport view = ScreenSystem.Viewport;
            SetDescriptionArea(new Rectangle(100, view.Height -100, view.Width -100, 50),
                new Color(11, 38, 40), new Color(29, 108, 117), new Point(10, 0), 0.5f);
        }

        void GameOverScreen_Removing(object sender, EventArgs e)
        {
            MenuEntries.Clear();
        }

        void GameOverScreen_Entering(object sender, TransitionEventArgs te)
        {
            //Slide effect from left to right
            float effect = (float)Math.Pow(te.percent - 1, 2) * -100;
            foreach (MenuEntry entry in MenuEntries)
            {
                entry.Acceleration = new Vector2(effect, 0);
                entry.Position = entry.InitialPosition + entry.Acceleration;
                entry.Scale = te.percent;
                entry.Opacity = te.percent;
            }

            TitlePosition = InitialTitlePosition - new Vector2(0, effect);
            TitleOpacity = te.percent;
        }

        void GameOverScreen_Exiting(object sender, TransitionEventArgs te)
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

            TitlePosition = InitialTitlePosition - new Vector2(0, effect);
            TitleOpacity = te.percent;
        }

        void play_Selected(object sender, EventArgs e)
        {
            ExitScreen();
            levelselect.ActivateScreen();
        }

        void quit_Selected(object sender, EventArgs e)
        {
            ScreenSystem.Game.Exit();
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            SpriteFont = content.Load<SpriteFont>(@"Fonts/menuFont");
            mouse = content.Load<Texture2D>(@"Images\mouseCursor");
            EnableMouse(mouse);

            play.SetPosition(new Vector2(100, 200), true);
            quit.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing + 5), play, true);


        }

        public override void UnloadContent()
        {
            SpriteFont = null;
        }
    }
}
