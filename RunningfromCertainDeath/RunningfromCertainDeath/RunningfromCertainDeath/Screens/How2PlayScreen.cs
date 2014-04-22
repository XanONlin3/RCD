using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace RunningfromCertainDeath.Screens
{
    public class How2PlayScreen : GameScreen
    {
        GameScreen prevScreen;
        SpriteFont font;

        List<Texture2D> helpTextures;
        int helpTextureCnt;

        int index; //current texture 

        public override bool AcceptsInput
        {
            get { return true; }
        }

        //Constructor
        public How2PlayScreen(GameScreen prev)
        {
            prevScreen = prev;
        }

        public override void InitializeScreen()
        {
            InputMap.NewAction("Finished", Keys.Escape);
            InputMap.NewAction("Next", Keys.Right);
            InputMap.NewAction("Prev", Keys.Left);

            helpTextureCnt = 3;
            index = 0;
            helpTextures = new List<Texture2D>(helpTextureCnt);

            EnableFade(Color.Black, 0.85f);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            font = content.Load<SpriteFont>(@"Fonts\helpFont");

         /*   for (int i = 0; i < helpTextureCnt; i++)
            {
                helpTextures.Add(content.Load<Texture2D>(
                    string.Format("Textures\\Help\\help_{0}", i+1)));
            } */
        }

        protected override void UpdateScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (InputMap.NewActionPress("Finished"))
            {
                ExitScreen();
                prevScreen.ActivateScreen();
            }

            if (InputMap.NewActionPress("Next"))
            {
                if(index < (helpTextureCnt- 1))
                {
                    index++;
                }
            }

            if (InputMap.NewActionPress("Prev"))
            {
                if(index > 0)
                {
                    index--;
                }
            }
        }

        protected override void DrawScreen(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;

          //  spriteBatch.Draw(helpTextures[index],
          //      new Rectangle(0, 0, 1280, 720), Color.White);
            spriteBatch.DrawString(font, "Press <- Arrows -> to advance to the next image", Vector2.Zero, Color.Aqua);
        }
    }
}
