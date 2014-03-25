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

namespace RunningfromCertainDeath
{
    public enum GameState
    {
        Splash, //no menu items
        MainMenu, //Single Player Game, Options, Credits
            Instructions,
            Options,
            Credits,
        Playing,
        Pause,
        GameOver
    }
    // MAIN
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        List<string> menuItems = new List<string>(); 
        //
        Menu mainMenu, playing, pause, gameOver;
        GameState currentGameState = new GameState();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Initialization
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mainMenu = new Menu("Running from Certain Death");
            mainMenu.AddMenuItem("Play", b => { if( b == Buttons.X) { currentGameState =GameState.Playing;}});
            mainMenu.AddMenuItem("Instructions", b => {if( b == Buttons.X) { currentGameState =GameState.Instructions;}});
            mainMenu.AddMenuItem("Options", b => { if( b == Buttons.X) { currentGameState =GameState.Options;}});
            mainMenu.AddMenuItem("Credits", b => { if (b == Buttons.X) { currentGameState = GameState.Credits;}});
            mainMenu.AddMenuItem("Exit",  b => { if (b == Buttons.X) { Exit();}});

            pause =new Menu("Paused");
            pause.AddMenuItem("Resume",  b => { if( b == Buttons.X) { currentGameState =GameState.Playing;}});
            pause.AddMenuItem("Quit", b => {if( b == Buttons.X) { currentGameState =GameState.MainMenu;}});

            pause =new Menu("Game Over");
            pause.AddMenuItem("MainMenu",  b => { if( b == Buttons.X) { currentGameState =GameState.MainMenu;}});
            pause.AddMenuItem("Quit", b => { if (b == Buttons.X) { Exit();}});



            base.Initialize();
        }

        // Load Content
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        // Unload Content
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Menu.Navigate(gameTime);

            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

/*
public void UpdateMenu(){
 * menuItems.clear(); //get rid of what ever is in menuItems
 *  switch(gameState)
 *  {
 *      case GameState.Menu: //add what ever menu options you would like to have for the main menu
 *      menuItems.Add("Play Game");
 *      munuItems.Add("Instructions");
 *      menuItems.Add("Credits");
 *      menuItems.Add("plaques");
 *      menuItems.Add("Exit Game");
 *      break;
 *      
 *      case GameState.Pause:
 *      menuItems.Add("Back To Main Menu");
 *      menuItems.Add("Exit");
 *      break;
 *  }
 * }

*/