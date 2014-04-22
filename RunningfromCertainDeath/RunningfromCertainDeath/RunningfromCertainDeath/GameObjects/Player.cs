using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunningfromCertainDeath.RCD_TileEngine;
using RunningfromCertainDeath.Animation;

namespace RunningfromCertainDeath.GameObjects
{
    public class Player : Entity
    {
        //isBleeding
        public bool isBleeding;

        // Initialize the player
        public void Initialize( Vector2 position)
        {
            // Set the player health
            health = 100;

            // By default player is Not bleeding
            isBleeding = false;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, Input.InputManager input)
        {
 	         base.LoadContent(content, input);    
        }

        // Update the player animation
        public override void Update(GameTime gameTime)
        {
        }

        // Draw the player
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
