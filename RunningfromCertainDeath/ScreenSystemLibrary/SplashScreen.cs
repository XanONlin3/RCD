using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScreenSystemLibrary
{
    public abstract class SplashScreen:IntroScreen
    {
        #region Fields and Properties
        //texture holds our logo
        public Texture2D Texture
        {
            get;
            set;
        }

        Color fadeColor;
        float fadePercent;
        #endregion

        protected SplashScreen() { }

        protected SplashScreen(Color fadeColor, float fadePercent)
        {
            this.fadeColor = fadeColor;
            this.fadePercent = fadePercent;
        }

        public override void InitializeScreen()
        {
            TransitionOnTime = TransitionOffTime = TimeSpan.FromSeconds(2.5);
            EnableFade(fadeColor, fadePercent);
        }

        public abstract override void LoadContent();

        protected override void DrawScreen(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;
            Viewport viewport = ScreenSystem.Game.GraphicsDevice.Viewport;

            //Centers the texture on the screen
            Vector2 centerTexture = new Vector2((viewport.Width / 2) - (Texture.Width / 2),
                (viewport.Height / 2) - (Texture.Height / 2));

            spriteBatch.Draw(Texture, centerTexture, Color.White * ScreenAlpha);
        }
    }
}
