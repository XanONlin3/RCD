using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace RunningfromCertainDeath
{
    class MenuItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Action<Buttons> Action { get; set; }
    }
}
