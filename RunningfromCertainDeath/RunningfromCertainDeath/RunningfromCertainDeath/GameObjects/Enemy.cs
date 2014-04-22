using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunningfromCertainDeath.Animation;
using RunningfromCertainDeath.RCD_TileEngine;
using Microsoft.Xna.Framework.Input;
using ScreenSystemLibrary;
using Microsoft.Xna.Framework.Content;

namespace RunningfromCertainDeath.GameObjects
{
    // Possible enemy states
    public enum EnemyState 
    {
       idle = 0, // default pathing
       ChasePlayer,
       AttackPlayer,
       Dead // X.X
    }
  

    class Enemy : Entity
    {
        public EnemyState currentState = EnemyState.idle;

        // Animation representing the enemy
        //public Animate EnemyAnimation;

        // The position of the enemy 
        public Vector2 Position;

        // The state of the Enemy
        public bool Active;

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;

        // The amount of damage the enemy inflicts on the player 
        public int Damage;

        // If player isbleeding = false? isBleeding = true : add stack?
        //public bool Player.isBleeding{ get; set; }
        public int stack;

        // Get the width of the enemy 
   /*     public int Width
        {
            get { return EnemyAnimation.FrameWidth; }
        }

        // Get the height of the enemy 
        public int Height
        {
            get { return EnemyAnimation.FrameHeight; }
        } */

        // The speed at which the enemy moves
        float enemyMoveSpeed;

        public void Initialize(Vector2 position)
        {
            // Load the enemy texture
            // Set the position of the enemy
            Position = position;

            // We initialize the enemy to be active so it will be update in the game
            Active = true;

            // Set the health of the enemy
            Health = 10;

            // Set the amount of damage the enemy can do
            Damage = 10;

            // Set how fast the enemy moves
            enemyMoveSpeed = 6f;

            // Set the bleeding effect
       /*     if (isBleeding)
                stack++;
            else
                isBleeding = true; */

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, Input.InputManager input)
        {
            base.LoadContent(content, input);
        }

        public override void Update(GameTime gameTime)
        {
            // Update Animation
         
            // If the enemy health reaches 0 then deactivateit
            if (Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe 
                // active game list
                Active = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
        }
    } 
} //NS
