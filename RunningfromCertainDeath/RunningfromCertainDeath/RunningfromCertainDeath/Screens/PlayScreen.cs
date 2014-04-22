using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RunningfromCertainDeath.GameObjects;
using RunningfromCertainDeath.Animation;

namespace RunningfromCertainDeath.Screens
{
    public class PlayScreen : GameScreen
    {
        SpriteFont font;
        Game1 game = new Game1();
        Input.InputManager input;

        SpriteStuff.Background background;
        RCD_TileEngine.Level level;

        //Character----------------
        //public static Texture2D playerTexture;
        //public static Animation.MobileSprite player;

        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // A movement speed for the player
        float playerMoveSpeed;

        Player boaz; 

        //Enemy--------------------
        public Texture2D enemyTexture;
        public Animation.MobileSprite enemy;
        List<Texture2D> enemies;

        //Item----------------------
 

        //Mouse mouse;

        public override bool AcceptsInput
        {
            get { return true; }
        }

        //Constructor
        public PlayScreen() //, Map m
        {
            level = new RCD_TileEngine.Level(game.graphics, new Vector2(200, 200));
        }

        public override void InitializeScreen()
        {

            InputMap.NewAction("Pause", Keys.Escape);
            Entering += new TransitionEventHandler(PlayScreen_Entering);

            background = new SpriteStuff.Background();

            //Initialize the player class
            //boaz = new Player();
        }

        void PlayScreen_Entering(object sender, TransitionEventArgs tea)
        {

        }

        public override void LoadContent()
        {
            ContentManager content = ScreenSystem.Content;
            font = content.Load<SpriteFont>(@"Fonts\gamefont");

            input = new Input.InputManager();

            //mouse = new Mouse(content.Load<Texture2D>(@"Images\mouseCursor"));

            level.LoadLevelText(1, content);

            background.LoadContent(content);

            // Load the player resources
            Texture2D playerTexture = content.Load<Texture2D>(@"Images\running3");

            Vector2 playerPos = new Vector2(ScreenSystem.Viewport.Bounds.X, ScreenSystem.Viewport.Bounds.Y +
                                                +ScreenSystem.Viewport.Bounds.Height / 2);

            //boaz.Initialize(playerPos);
            //boaz.LoadContent(content, input);
            //boaz.texture = content.Load<Texture2D>(@"Images\running3"); 

         /*   enemyTexture = content.Load<Texture2D>(@"Images\enemy_1");
            enemy = new Animation.MobileSprite(enemyTexture);
            enemy.Sprite.AddAnimation("path", 0, 0, 52, 38, 5, 0.1f);
            enemy.Sprite.AutoRotate = true;
            enemy.Position = new Vector2(300, 300);
            enemy.Target = enemy.Position;
            enemy.AddPathNode(new Vector2(200, 200));
            enemy.AddPathNode(new Vector2(400, 200));
            enemy.AddPathNode(new Vector2(400, 400));
            enemy.AddPathNode(new Vector2(200, 400));
            enemy.Speed = 3;
            enemy.LoopPath = true; 

            playerTexture = content.Load<Texture2D>(@"Images\running3");
            player = new Animation.MobileSprite(playerTexture);
            player.Sprite.AddAnimation("leftstop", 0, 0, 30, 34, 1, 0.1f);
            player.Sprite.AddAnimation("left", 30, 0, 30, 34, 8, 0.1f);
            player.Sprite.AddAnimation("rightstop", 240, 34, 30, 34, 1, 0.1f);
            player.Sprite.AddAnimation("right", 0, 34, 30, 34, 8, 0.1f);
            player.Sprite.CurrentAnimation = "rightstop";
            player.Position = new Vector2(100, 300);
            player.Sprite.AutoRotate = false;
            player.IsPathing = false;
            player.IsMoving = false;*/
        }

        public override void UnloadContent()
        {
            font = null;
        }

        protected override void UpdateScreen(GameTime gameTime)
        {
            
            /*KeyboardState ks = Keyboard.GetState();
            bool leftkey = ks.IsKeyDown(Keys.A);
            bool rightkey = ks.IsKeyDown(Keys.D);

            if (leftkey)
            {
                if (player.Sprite.CurrentAnimation != "left")
                {
                    player.Sprite.CurrentAnimation = "left";
                }
                player.Sprite.MoveBy(-2, 0);
            }

            if (rightkey)
            {
                if (player.Sprite.CurrentAnimation != "right")
                {
                    player.Sprite.CurrentAnimation = "right";
                }
                player.Sprite.MoveBy(2, 0);
            }

            if (!leftkey && !rightkey)
            {
                if (player.Sprite.CurrentAnimation == "left")
                {
                    player.Sprite.CurrentAnimation = "leftstop";
                }
                if (player.Sprite.CurrentAnimation == "right")
                {
                    player.Sprite.CurrentAnimation = "rightstop";
                }
            }

            // Save the previous state of the keyboard to determine single key/button presses
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and store it
            currentKeyboardState = Keyboard.GetState();

            //Update the player && enemies
            //boaz.Update(gameTime);
            //enemy.Update(gameTime);*/
            level.Update(gameTime);
            //player.Update(gameTime);
            background.Update(gameTime);

        }

        public override void HandleInput()
        {
            if (InputMap.NewActionPress("Pause"))
            {
                FreezeScreen();
                ScreenSystem.AddScreen(new PauseScreen(this));
            }
        }

        protected override void DrawScreen(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;

            background.Draw(spriteBatch);
            level.Draw(spriteBatch);
            //player.Draw(spriteBatch);
            //boaz.Draw(spriteBatch);
            //enemy.Draw(spriteBatch);
        }
        
    }


}