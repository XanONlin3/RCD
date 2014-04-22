using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScreenSystemLibrary
{
    public class TransitionEventArgs : EventArgs
    {
        public float percent, time;
        public TransitionEventArgs(float percent, float time)
        {
            this.percent = percent;
            this.time = time;
        }
    }
    public enum TransitionState
    {
        On,
        None,
        Off,
    }
    /// <summary>
    /// The current state of our screen
    /// </summary>
    public enum ScreenState
    {
        //Update + Draw but transitions on
        TransitionOn,
        //Update + Draw
        Active,
        //Update + Draw but transitions off
        TransitionOff,
        //Updates, but does not draw
        Hidden,
        //Draws, but does not update
        Frozen,
        //Does not Update or Draw
        Inactive,
    }
    public abstract class GameScreen
    {
        #region Delegate
        public delegate void TransitionEventHandler(object sender, TransitionEventArgs tea);
        #endregion

        #region Fields and Properties
        public TimeSpan TransitionOnTime
        {
            get;
            set;
        }

        public TimeSpan TransitionOffTime
        {
            get;
            set;
        }

        public float TransitionPercent
        {
            get;
            private set;
        }

        float transitionDirection = 1;
        public float TransitionDirection
        {
            get { return transitionDirection; }
        }

        float transitionMultiplier = 1;
        public float TransitionMultiplier
        {
            get { return transitionMultiplier; }
            protected set { transitionMultiplier = value; }
        }

        TransitionState transitionState = TransitionState.On;
        public TransitionState TransitionState
        {
            get { return transitionState; }
        }

        public virtual float ScreenAlpha
        {
            get { return 1.0f; }
        }

        ScreenState state = ScreenState.TransitionOn;

        public ScreenState State
        {
            get { return state; }
            protected set { state = value; }
        }

        public bool IsActive
        {
            get
            {
                return (state == ScreenState.TransitionOn || state == ScreenState.TransitionOff
                    || state == ScreenState.Active);
            }
        }

        public ScreenSystem ScreenSystem
        {
            get;
            set;
        }

        public abstract bool AcceptsInput
        {
            get;
        }

        bool isContentLoaded = false;

        public bool IsContentLoaded
        {
            get { return isContentLoaded; }
        }

        bool isContentUnloaded = false;

        public bool IsContentUnloaded
        {
            get { return isContentUnloaded; }
        }

        public InputMap InputMap
        {
            get;
            private set;
        }
        #endregion

        #region Fade Data
        Texture2D fadeTexture;
        Color fadeColor;
        float fadeAmount;
        bool fadeIsEnabled = false;
        #endregion

        #region Events

        //TransitionOn event
        public event TransitionEventHandler Entering;

        //TransitionOff event
        public event TransitionEventHandler Exiting;

        //Removed the screen
        public event EventHandler Removing;
        #endregion

        #region Initialization
        public void Initialize()
        {
            InputMap = new InputMap();
        }

        public abstract void InitializeScreen();

        public virtual void LoadContent()
        {
            isContentLoaded = true;
        }

        public virtual void UnloadContent()
        {
            isContentUnloaded = true;
        }
        #endregion

        #region Update and Draw
        public void Update(GameTime gameTime)
        {
            //Update the input system
            InputSystem.Update(gameTime);

            //If the screen state is either frozen or inactive, do not do any updating.
            //This is needed in case a screen sets the status before base.Update();
            if (state == ScreenState.Frozen || state == ScreenState.Inactive)
            {
                return;
            }
            if (state == ScreenState.TransitionOn)
            {
                float currentTransitionTime =
                    CalculateTransitionTime(TransitionOnTime, gameTime);

                //Transition on will be going from 0 to 1.0, so we add
                TransitionPercent += currentTransitionTime *
                    transitionMultiplier;

                if (TransitionPercent >= 1.0)
                {
                    TransitionPercent = 1.0f;
                    state = ScreenState.Active;
                }
                else
                {
                    if (Entering != null)
                    {
                        Entering(this, new TransitionEventArgs(
                            TransitionPercent, currentTransitionTime));
                    }
                }
            }
            else if (state == ScreenState.TransitionOff)
            {
                float currentTransitionTime =
                    CalculateTransitionTime(TransitionOffTime, gameTime);

                //Transition off will be going from 1.0 to 0, so we subtract
                TransitionPercent -= currentTransitionTime *
                    transitionMultiplier;

                if (TransitionPercent <= 0)
                {
                    TransitionPercent = 0;
                    Remove();
                }
                else
                {
                    if (Exiting != null)
                    {
                        Exiting(this, new TransitionEventArgs(
                            TransitionPercent, currentTransitionTime));
                    }
                }
            }

            else if (state == ScreenState.Active || state == ScreenState.Hidden)
            {
                UpdateScreen(gameTime);
            }
        }

        private float CalculateTransitionTime(TimeSpan transitionTime, GameTime gameTime)
        {
            if (transitionTime == TimeSpan.Zero)
            {
                return 1;
            }
            else
            {
                return (float)(gameTime.ElapsedGameTime.TotalSeconds
                        / transitionTime.TotalSeconds);
            }
        }

        protected abstract void UpdateScreen(GameTime gameTime);



        public virtual void HandleInput() { }

        public void Draw(GameTime gameTime)
        {
            if (state != ScreenState.Inactive && state != ScreenState.Hidden)
            {
                SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;
                spriteBatch.Begin();
                if (fadeIsEnabled)
                {
                    Viewport view = ScreenSystem.Viewport;
                    spriteBatch.Draw(fadeTexture, new Rectangle(0, 0, view.Width, view.Height), fadeColor);

                }
                DrawScreen(gameTime);
                spriteBatch.End();
            }
        }

        protected abstract void DrawScreen(GameTime gameTime);
        #endregion

        #region Screen Methods
        public void ExitScreen()
        {
            state = ScreenState.TransitionOff;
            transitionDirection = -1;
        }
        private void Remove()
        {
            ScreenSystem.RemoveScreen(this);
            if (Removing != null)
                Removing(this, EventArgs.Empty);
        }

        public void FreezeScreen()
        {
            state = ScreenState.Frozen;
        }

        public void ActivateScreen()
        {
            if (state != ScreenState.Inactive && state != ScreenState.Active)
                state = ScreenState.Active;
        }

        public void EnableFade(Color c, float percentage)
        {
            percentage = MathHelper.Clamp(percentage, 0, 1);
            fadeAmount = percentage;
            fadeTexture = new Texture2D(ScreenSystem.GraphicsDevice, 1, 1);
            //c *= percentage;
            Color[] cArray = new Color[] { c };
            //fadeColor = c;
            fadeTexture.SetData<Color>(cArray);
            fadeColor = c * percentage;
            fadeIsEnabled = true;
        }

        public void DisableFade()
        {
            fadeTexture = null;
            fadeColor = Color.White;
            fadeIsEnabled = false;
        }
        #endregion
    }
}
