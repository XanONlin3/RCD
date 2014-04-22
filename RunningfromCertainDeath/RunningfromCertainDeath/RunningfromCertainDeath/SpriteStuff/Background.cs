using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RunningfromCertainDeath.SpriteStuff
{
    class Background
    {

        public Texture2D texture1, texture2;
        public Vector2 mainMenuBG, leaves;
        public int speed;

        //Constructor
        public Background()
        {
            speed = 3; // Speed of falling leaves?

        }

        // LoadContent
        public void LoadContent(ContentManager Content)
        {
            texture1 = Content.Load<Texture2D>(@"Maps/CityLevel/sword city");
            //texture2 = Content .Load<Texture2D>(@"Images/GameScreen_FULL"); //Leaves
            mainMenuBG = new Vector2(0, 0);
            //leaves = new Vector2(0, 0);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Setting BG speed
            leaves.Y -= speed;
            //skyPosFollow.X -= speed;

            // Loop Falling leaves
            if (leaves.Y <= -720){
                leaves.Y = 0;
                leaves.X = 720;
            }
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture1, mainMenuBG, Color.White);
            //spriteBatch.Draw(texture2, skyPosFollow, Color.White);
        }
    }
}
