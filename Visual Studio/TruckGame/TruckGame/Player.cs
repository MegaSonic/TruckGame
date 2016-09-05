#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace TruckGame
{
    public class Player : GameObject, ICollideable
    {
        // public Texture2D PlayerTexture;
        public Animation playerAnimation;

        public bool active;
        public int health;

        public Game1 activeGame;

        public float playerMoveSpeed = 300.0f;
        public float playerTurnSpeed = 1.0f;

        private float dodgeTimer = 0f;
        public float dodgeCooldownTimer = 0f;
        public float dodgeLength = 0.16f;
        public float dodgeCooldownLength = 0.3f;
        public float dodgeSpeed = 1000.0f;

        public bool isDodgeRolling = false;

        public Player()
        {
            
        }

        public void Start(Game1 game, Animation animation, Vector2 position)
        {
            playerAnimation = animation;
            this.Position = position;
            active = true;
            health = 100;
            activeGame = game;
            tag = "Player";
            playerAnimation.depth = 0.1f;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

#if (DEBUG)
            Debug.WriteLine(deltaTime);
#endif
            if (dodgeTimer > 0f) dodgeTimer -= deltaTime;
            if (dodgeCooldownTimer > 0f && !isDodgeRolling) dodgeCooldownTimer -= deltaTime;

            if (dodgeTimer < 0f) isDodgeRolling = false;


            // this.X += activeGame.currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed * deltaTime;
            // this.Y -= activeGame.currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed * deltaTime;

            
            

            bool left = false, right = false;

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Left) || activeGame.currentKeyboardState.IsKeyDown(Keys.A) || activeGame.currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                if (!isDodgeRolling)
                {
                    this.X -= playerMoveSpeed * deltaTime;
                }
                else
                {
                    this.X -= dodgeSpeed * deltaTime;
                }
                Rotation = (float) Math.PI;
                left = true;
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Right) || activeGame.currentKeyboardState.IsKeyDown(Keys.D) || activeGame.currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                if (!isDodgeRolling)
                {
                    this.X += playerMoveSpeed * deltaTime;
                }
                else 
                {
                    this.X += dodgeSpeed * deltaTime;
                }
                Rotation = 0f;
                right = true;
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Up) || activeGame.currentKeyboardState.IsKeyDown(Keys.W) || activeGame.currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                if (!isDodgeRolling)
                {
                    this.Y -= playerMoveSpeed * deltaTime;
                }
                else
                {
                    this.Y -= dodgeSpeed * deltaTime;
                }
                Rotation = (float)Math.PI * 3 / 2;
                if (left) Rotation = (float)Math.PI * 5 / 4;
                else if (right) Rotation = (float)Math.PI * 7 / 4;
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Down) || activeGame.currentKeyboardState.IsKeyDown(Keys.S) || activeGame.currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                if (!isDodgeRolling)
                {
                    this.Y += playerMoveSpeed * deltaTime;
                }
                else
                {
                    this.Y += dodgeSpeed * deltaTime;
                }
                Rotation = (float) Math.PI / 2;
                if (left) Rotation = (float) Math.PI * 3/4;
                else if (right) Rotation = (float)Math.PI * 1 / 4;
            }

            // Dodge Roll
            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Z) && activeGame.previousKeyboardState.IsKeyUp(Keys.Z) && dodgeCooldownTimer <= 0)
            {
                isDodgeRolling = true;
                dodgeTimer = dodgeLength;
                dodgeCooldownTimer = dodgeCooldownLength;
                
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.X) && activeGame.previousKeyboardState.IsKeyUp(Keys.X))
            {
                // Taunt
            }

            this.X = MathHelper.Clamp(this.X, playerAnimation.FrameWidth, activeGame.GraphicsDevice.Viewport.Width);
            this.Y = MathHelper.Clamp(this.Y, playerAnimation.FrameHeight, activeGame.GraphicsDevice.Viewport.Height);

            playerAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
        }

        

        public int Width
        {
            get { return playerAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return playerAnimation.FrameHeight; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; playerAnimation.Position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; playerAnimation.Position.Y = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; playerAnimation.Position = value; }
        }

        public float Rotation
        {
            get { return playerAnimation.angle; }
            set { playerAnimation.angle = value; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)this.X - playerAnimation.FrameWidth / 2, (int)this.Y - playerAnimation.FrameHeight / 2, playerAnimation.FrameWidth, playerAnimation.FrameHeight);
            }
        }

        public bool IsCurrentlyCollideable
        {
            get
            {
                if (isDodgeRolling) return false;
                else return true;
            }
        }

        public void DodgeRoll()
        {

        }

        public void Collided(GameObject collidedWith)
        {
            if (collidedWith.tag == "Truck" )
            {
                //ICollideable possibleCollideable = collidedWith as ICollideable;
                //if (possibleCollideable != null)
                //{
                    //if (possibleCollideable.IsCurrentlyCollideable)
                    //{
                        // Kill the player
                        activeGame.Reset();
                    //}
                //}

            }
        }
    }
}