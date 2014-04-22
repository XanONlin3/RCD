using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScreenSystemLibrary
{
    public abstract class IntroScreen:GameScreen
    {
        #region Fields and Properties
        //How long the intro screen will be shown
        public TimeSpan ScreenTime
        {
            get;
            set;
        }
        
        public override float ScreenAlpha
        {
            get { return TransitionPercent; }
        }

        public override bool AcceptsInput
        {
            get { return true; }
        }
        #endregion

        protected IntroScreen() { }

        public abstract override void InitializeScreen();

        protected override void UpdateScreen(GameTime gameTime)
        {
            //Update the timer
            ScreenTime = ScreenTime.Subtract(gameTime.ElapsedGameTime);

            if (ScreenTime.TotalSeconds <= 0)
                ExitScreen();
        }

        protected abstract override void DrawScreen(GameTime gameTime);
    }
}
