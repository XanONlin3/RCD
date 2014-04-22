using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RunningfromCertainDeath.SpriteStuff
{
    class AnimateSprite : Sprite
    {
        int startFrame;
        int endFrame;
        Point currentFrame;
        int delay;
        float timePassed;
        int textureWidth;

        int spriteWidth, spriteHeight;

        // Constructor
        public AnimateSprite(Texture2D texture, Vector2 position, int spriteWidth, int spriteHeight, int startFrame, int endFrame, Point currentFrame, int millisecond_delay)
            : base(texture, position)
        {
            this.textureWidth = texture.Width;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.delay = millisecond_delay;

            _sourceRect = new Rectangle((int)currentFrame.X * spriteWidth, (int)currentFrame.Y * spriteHeight,
                        spriteWidth, spriteHeight);
        }

        public void Update(GameTime gameTime)
        {
            this.timePassed += gameTime.ElapsedGameTime.Milliseconds;

            if (this.timePassed >= this.delay)
            {
                currentFrame.X++;

                if (currentFrame.X * spriteWidth >= this.textureWidth)
                    currentFrame.X = startFrame;

                _sourceRect = new Rectangle((int)currentFrame.X * spriteWidth, (int)currentFrame.Y * spriteHeight,
                        spriteWidth, spriteHeight);

                this.timePassed = 0;
            }
        }
    }
}
