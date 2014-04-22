using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RunningfromCertainDeath.Screens
{
    class JKTaLogoScreen : SplashScreen
    {
        public JKTaLogoScreen()
            : base() { }

        public JKTaLogoScreen(Color fadeColor, float fadePercent)
            : base(fadeColor, fadePercent) 
            {
                // Allow transitions
                TransitionOnTime = TimeSpan.FromSeconds(1);
                TransitionOffTime = TimeSpan.FromSeconds(0.5);
            }

        public override void InitializeScreen()
        {
            ScreenTime = TimeSpan.FromSeconds(1);
            Removing += new EventHandler(RemovingScreen);
            base.Initialize();
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            Texture = content.Load<Texture2D>(@"Images\JKTaDemoLogo");
        }

        public override void UnloadContent()
        {
            Texture = null;
        }

        void RemovingScreen(object sender, EventArgs e)
        {
            // Screen to load when this one is done
            ScreenSystem.AddScreen(new MainMenuScreen());
        }
    }
}
