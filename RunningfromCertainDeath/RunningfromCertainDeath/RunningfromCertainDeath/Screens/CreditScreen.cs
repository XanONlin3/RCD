using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace RunningfromCertainDeath.Screens
{
    class CreditScreen : GameScreen
    {
        string credits;
        SpriteFont font;
        int scrollSpeed;
        GameScreen prevScreen;

        public override bool AcceptsInput
        {
            get { return true; }
        }

        //Constructor
        public CreditScreen(GameScreen prev)
        {
            prevScreen = prev;
        }

        public override void InitializeScreen()
        {
            InputMap.NewAction("Finished", Keys.Escape);

            EnableFade(Color.Black, 0.85f);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            font = content.Load<SpriteFont>(@"fonts\creditFont");
        }
        protected override void UpdateScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputMap.NewActionPress("Finished"))
            {
                ExitScreen();
                prevScreen.ActivateScreen();
            }
        }

        public string createCredits()
        {
            //Use the StringBuilder class to create a nice looking string to display
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------Credits----------");
            /*  sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine("");
              sb.AppendLine(""); */

            credits = sb.ToString();
            return credits;
        }

        protected override void DrawScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;
            credits = createCredits();
            spriteBatch.DrawString(font, credits, new Vector2((ScreenSystem.Viewport.Bounds.Width - font.MeasureString(credits).Length()) / 2, 25), Color.White);
        }
    }
}

