using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RunningfromCertainDeath.SpriteStuff
{
    public class Sprite
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Rectangle _sourceRect;

        // Constructor
        public Sprite(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;

            _sourceRect = _texture.Bounds;
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
        }
    }
}
