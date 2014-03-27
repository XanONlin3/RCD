using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCD.GameScreen_stuff
{
    class GameScreen
    {
        // Transition states
        public enum ScreenState
        {
            TransitionOn,
            Active,
            TransitionOff,
            Hidden
        }

        // Indication of if screen is pop-up 
        public abstract class GameScreen
        {
            //if pop-up screen
            public bool IsPopup
            {
                get { return isPopup; }
                protected set { isPopup = value; }
            }

            bool isPopup = false;
        }//GS Class

        // Indication of how long the screen takes to transition on activation
        public TimeSpan TransitionONTime
        {
            get { return TransitionONTime; }
            protected set { TransitionONTime = value; }
        }

        TimeSpan transitionONTime = TimeSpan.Zero;

        // Indication of how long the screen takes to transition on deactivate
        public TimeSpan TransitionOFFTime
        {
            get { return TransitionOFFTime; }
            protected set { TransitionOFFTime = value; }
        }

        TimeSpan transitionOFFTime = TimeSpan.Zero;

        // Get current posiion of the screen transition
        public float TransitionPosition
        {
            get { return TransitionPosition; }
            protected set {TransitionPosition =value; }
        }
        float transitionPosition = 1;

        // Get current Alpha of screen transition ranging
        // from 1 (fully active, no transition) to 0 (transitioned
        // fully off to nothing).
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }




    }//Class
}//NS
