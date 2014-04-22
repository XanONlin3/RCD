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
    class ControlsScreen : GameScreen
    {
        string info;
        SpriteFont font;
        Texture2D texture;
        GameScreen prevScreen;

        public override bool AcceptsInput
        {
            get { return true; }
        }

        //Constructor
        public ControlsScreen(GameScreen prev)
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
            font = content.Load<SpriteFont>(@"Fonts\helpFont");
            texture = content.Load<Texture2D>(@"Textures\Help\keyboardLayout");
        }
        protected override void UpdateScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputMap.NewActionPress("Finished"))
            {
                ExitScreen();
                prevScreen.ActivateScreen();
            }
        }

        public string createInfo()
        {
            //Use the StringBuilder class to create a nice looking string to display
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------Keyboard Layout----------");
            info = sb.ToString();
            return info;
        }

        protected override void DrawScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;
            info = createInfo();
            spriteBatch.Draw(texture, new Vector2((ScreenSystem.Viewport.Bounds.Width - texture.Width) /2,( ScreenSystem.Viewport.Bounds.Height - texture.Height) /2), Color.White);
            spriteBatch.DrawString(font, info, new Vector2((ScreenSystem.Viewport.Bounds.Width - font.MeasureString(info).Length()) / 2, 25), Color.White);
        }
    }
}

