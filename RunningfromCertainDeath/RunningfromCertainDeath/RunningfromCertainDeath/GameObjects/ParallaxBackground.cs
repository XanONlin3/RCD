using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RunningfromCertainDeath.GameObjects
{
    /*
     * We’ll make two of these ParallaxBackground objects inside the playScreen class 
     * and then a basic Texture2D for the static part.
     */

    class ParallaxBackground
    {
        // The image representing the parallaxing background
        Texture2D texture;

        // An array of positions of the parallaxing background
        Vector2[] positions;

        // The speed which the background is moving
        int speed;

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
        {
            // Load the background texture
            texture = content.Load<Texture2D>(texturePath);

            // Set the speed of the background
            this.speed = speed;

            // If we divide the screen with the texture width then we can determine the number of tiles need.
            // We add 1 to it so that we won't have a gap in the tiling
            positions = new Vector2[screenWidth / texture.Width + 1];

            // Set the initial positions of the parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }

        public void Update()
        {
            // Update the positions of the background
            for (int i = 0; i < positions.Length; i++)
            {
                // Update the position of the screen by adding the speed
                positions[i].X += speed;
                // If the speed has the background moving to the left
                if (speed <= 0)
                {
                    // Check the texture is out of view then put that texture at the end of the screen
                    if (positions[i].X <= -texture.Width)
                    {
                        positions[i].X = texture.Width * (positions.Length - 1);
                    }
                }

                // If the speed has the background moving to the right
                else
                {
                    // Check if the texture is out of view then position it to the start of the screen
                    if (positions[i].X >= texture.Width * (positions.Length - 1))
                    {
                        positions[i].X = -texture.Width;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture, positions[i], Color.White);
            }
        }
    }
}


/*
    // Image used to display the static background
    Texture2D mainBackground;

    // Parallaxing Layers
    ParallaxBackground bgLayer1;
    ParallaxBackground bgLayer2;

    in Initialize()
    bgLayer1 = new ParallaxBackground();
    bgLayer2 = new ParallaxBackground();
  
    in LoadContent()
    // Load the parallaxing background
    bgLayer1.Initialize(content, "Images\Backgrounds\bgLayer1", ScreenSystem.Viewport.Width, -1);
    bgLayer2.Initialize(content, "Images\Backgrounds\bgLayer2", ScreenSystem.Viewport.Width, -2);

    mainBackground = Content.Load<Texture2D>("Images\Backgrounds\mainbackground");
    
    in Update()
    // Update the parallaxing background
    bgLayer1.Update();
    bgLayer2.Update();
  
    in Draw()
    spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

    // Draw the moving background
    bgLayer1.Draw(spriteBatch);
    bgLayer2.Draw(spriteBatch);

*/