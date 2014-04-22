using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ScreenSystemLibrary
{
    public class ScreenSystem : DrawableGameComponent
    {
        #region Fields
        //List of current screens in the manager
        List<GameScreen> screens = new List<GameScreen>();

        //Another list dedicated to the screens that will be 
        //updated in the current game loop.
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        //Is the screen manager initialized?
        bool isInitialized;
        #endregion

        #region Properties
        /// <summary>
        /// Spritebatch for 2D drawings
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the content manager
        /// </summary>
        public ContentManager Content
        {
            get { return Game.Content; }
        }

        /// <summary>
        /// Gets the viewport object
        /// </summary>
        public Viewport Viewport
        {
            get { return GraphicsDevice.Viewport; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor, initializes the manager
        /// </summary>
        public ScreenSystem(Game game)
            : base(game)
        {
            base.Initialize();

            //Creates a new InputSystem
            isInitialized = true;
        }

        /// <summary>
        /// Initialize the spriteBatch and screen dedicated content.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //load screen dedicated content
            foreach (GameScreen screen in screens)
            {
                // If our content of the given screen is not loaded
                // call the load content method.
                if (!screen.IsContentLoaded)
                {
                    screen.LoadContent();
                }
            }
        }

        /// <summary>
        /// Unload screen dedicated content
        /// </summary>
        protected override void UnloadContent()
        {
            //Tells the screen to unload their content.
            foreach (GameScreen screen in screens)
            {
                // If the screen's content is not unloaded
                // already, call the unload content method
                if (!screen.IsContentUnloaded)
                {
                    screen.UnloadContent();
                }
            }
        }
        #endregion

        #region Update and Draw

        /// <summary>
        /// Update manager and screens
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            //clear out the screensToUpdate list to copy the screens list
            //this allows us to add or remove screens without complaining.
            screensToUpdate.Clear();

            //There are no screens to display, so we cannot continue
            //Therefore, quit the game
            if (screens.Count == 0)
                this.Game.Exit();

            // Add all the screens to the screensToUpdate list
            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            if (!Game.IsActive)
            {
                //Not in focus logic, delete this 'if' and 'else' if you want to remove
                //the functionality
            }
            else
            {
                while (screensToUpdate.Count > 0)
                {
                    GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                    screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                    //Update the screen unless its frozen or inactive
                    if (screen.State != ScreenState.Frozen
                        && screen.State != ScreenState.Inactive)
                    {
                        screen.Update(gameTime);
                    }

                    if (screen.IsActive && screen.AcceptsInput)
                    {
                        screen.HandleInput();
                    }
                }
            }
        }

        /// <summary>
        /// Tells each screen to draw
        /// </summary>
        /// <param name="gameTime">Time object to pass to the screens</param>
        public override void Draw(GameTime gameTime)
        {
            if (!Game.IsActive)
            {
                //Not in focus logic, delete this 'if' and 'else' if you want to remove
                //the functionality
            }
            else
            {
                foreach (GameScreen screen in screens)
                {
                    //Tells the current screen to draw if its not hidden
                    if (screen.State == ScreenState.Hidden)
                        continue;

                    screen.Draw(gameTime);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a screen to the manager
        /// </summary>
        /// <param name="screen">The screen to be added</param>
        public void AddScreen(GameScreen screen)
        {
            //Sets the reference to the screen manager on the screen
            screen.ScreenSystem = this;

            //If the screen manager is initialized, perform initialize operations 
            //for the screens.
            if (this.isInitialized)
            {
                screen.Initialize();
                screen.InitializeScreen();
                screen.LoadContent();
            }

            //Finally, add the screen to the list.
            screens.Add(screen);
        }

        /// <summary>
        /// Removed the desired screen from the system
        /// </summary>
        /// <param name="screen">The screen we wish to remove</param>
        public void RemoveScreen(GameScreen screen)
        {
            //If the screen manager is initialized, unload the screen content.
            if (this.isInitialized && !screen.IsContentUnloaded)
            {
                screen.UnloadContent();
            }

            //Finally, remove the screen from both lists.
            screens.Remove(screen);
        }
        #endregion
    }
}
