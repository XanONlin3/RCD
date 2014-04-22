using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RunningfromCertainDeath.GameObjects
{
    public class Entity 
    {
        protected int health;
        //public Animation.Animate moveAnimation;
        protected float moveSpeed;

        protected ContentManager content;
        public Texture2D texture { get; set; }

        protected Vector2 position = Vector2.Zero;

        public virtual void LoadContent(ContentManager content, Input.InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");

            //moveAnimation.LoadContent(content, texture, position);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
