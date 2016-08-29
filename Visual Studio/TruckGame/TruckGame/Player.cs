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
    class Player : GameObject
    {
        // public Texture2D PlayerTexture;
        public Animation playerAnimation;

        public bool active;
        public int health;

        public Game1 activeGame;

        public float playerMoveSpeed = 100.0f;
        public float playerTurnSpeed = 1.0f;

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
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            this.X += activeGame.currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed * deltaTime;
            this.Y -= activeGame.currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed * deltaTime;

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Left) || activeGame.currentKeyboardState.IsKeyDown(Keys.A) || activeGame.currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                this.X -= playerMoveSpeed * deltaTime;
                Debug.WriteLine("Left");
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Right) || activeGame.currentKeyboardState.IsKeyDown(Keys.D) || activeGame.currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                this.X += playerMoveSpeed * deltaTime;
                Debug.WriteLine("Right");
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Up) || activeGame.currentKeyboardState.IsKeyDown(Keys.W) || activeGame.currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                this.Y -= playerMoveSpeed * deltaTime;
                Debug.WriteLine("Up");
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Down) || activeGame.currentKeyboardState.IsKeyDown(Keys.S) || activeGame.currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                this.Y += playerMoveSpeed * deltaTime;
                Debug.WriteLine("Down");
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Q) || activeGame.currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                this.Rotation -= playerTurnSpeed * deltaTime;
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.E) || activeGame.currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                this.Rotation += playerTurnSpeed * deltaTime;
            }


            this.X = MathHelper.Clamp(position.X, 0, activeGame.GraphicsDevice.Viewport.Width - Width);
            this.Y = MathHelper.Clamp(position.Y, 0, activeGame.GraphicsDevice.Viewport.Height - Height);

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
    }
}