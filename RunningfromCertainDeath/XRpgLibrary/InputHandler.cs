using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace XRpgLibrary
{
    public partial class InputHandler : Component
    {
        public InputHandler()
        {
            InitializeComponent();
        }

        public InputHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
