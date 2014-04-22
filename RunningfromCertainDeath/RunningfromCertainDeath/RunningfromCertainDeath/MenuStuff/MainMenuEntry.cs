using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;

namespace RunningfromCertainDeath.MenuStuff
{
    public class MainMenuEntry : MenuEntry
    {
        public MainMenuEntry(MenuScreen menu, string title, string description)
            : base(menu, title)
        {
            EntryDescription = description;
        }

        public override void AnimateHighlighted(GameTime gametime)
        {
            // Animate the active entry
            float pulse = (float)(Math.Cos(gametime.TotalGameTime.TotalSeconds * 3) + 1);
            Scale = 1 + pulse * 0.05f;
        }

        public override void Update(GameTime gametime)
        {
            Position = new Vector2(InitialPosition.X, InitialPosition.Y);
        }
    }
}
