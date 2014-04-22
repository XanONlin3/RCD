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
    public class PlaqueScreen : GameScreen
    {
        GameScreen prevScreen;
        SpriteFont font;

        List<Texture2D> plaqueTextures;
        int plaqueTextureCnt;

        int index; //current texture 

        public override bool AcceptsInput
        {
            get { return true; }
        }

        //Constructor
        public PlaqueScreen(GameScreen prev)
        {
            prevScreen = prev;
        }

        public override void InitializeScreen()
        {
            InputMap.NewAction("Finished", Keys.Escape);
            InputMap.NewAction("Next", Keys.Right);
            InputMap.NewAction("Prev", Keys.Left);

            plaqueTextureCnt = 12;
            index = 0;
            plaqueTextures = new List<Texture2D>(plaqueTextureCnt);

            EnableFade(Color.Black, 0.85f);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            font = content.Load<SpriteFont>(@"Fonts\menuFont");

            for (int i = 0; i < plaqueTextureCnt; i++)
            {
                plaqueTextures.Add(content.Load<Texture2D>(
                    string.Format("Textures\\Plaques\\plaque_{0}", i+1)));
            }
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
                if(index < (plaqueTextureCnt- 1))
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
            string text = "{" + (index + 1) + "}"; 

            spriteBatch.Draw(plaqueTextures[index],
                new Rectangle((ScreenSystem.Viewport.Bounds.Width - 165) /2, (ScreenSystem.Viewport.Bounds.Height - 251) /2, 502, 330), Color.White);
            spriteBatch.DrawString(font, "Press <- Arrows -> to advance to the next plaque", Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, text, new Vector2((ScreenSystem.Viewport.Bounds.Width - font.MeasureString(text).Length())/ 2, 650), Color.White);
        }
    }
}
