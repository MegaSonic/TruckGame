using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    public class Truck : GameObject, ICollideable
    {
        public bool isInvincible;

        public static Texture2D truckTexture;

        public Animation truckAnimation;

        public bool active;
        public int health;

        public bool justSpawned = true;

        public Game1 activeGame;

        public float truckMoveSpeed = 200.0f;

        public Vector2 startPosition;
        public Vector2 targetPosition;

        public Truck(Game1 game, Vector2 position)
        {
            // Debug.WriteLine(truckTexture == null);
            if (truckTexture == null)
            {
                truckTexture = game.Content.Load<Texture2D>("truck_sheet");
            }

            truckAnimation = new Animation();
            truckAnimation.Initialize(truckTexture, Vector2.Zero, 111, 92, 1, 1000, Color.White, 1f, true);

            this.Position = position;
            active = true;
            health = 100;
            activeGame = game;
            tag = "Truck";
            
            
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)this.X - truckAnimation.FrameWidth / 2, (int)this.Y - truckAnimation.FrameHeight / 2, truckAnimation.FrameWidth, truckAnimation.FrameHeight);
            }
        }

        public void Collided(GameObject collidedWith)
        {
            if (!isInvincible)
            {

            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            truckAnimation.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 normalized = Vector2.Normalize(targetPosition - startPosition);
            normalized *= truckMoveSpeed;
            this.X += normalized.X * deltaTime;
            this.Y += normalized.Y * deltaTime;

            if (justSpawned == true)
            {
                if (this.X > 0 && this.X < activeGame.GraphicsDevice.Viewport.Width && this.Y > 0 && this.Y < activeGame.GraphicsDevice.Viewport.Height)
                {
                    // Truck is in bounds
                    isInvincible = false;
                }
            }

            // Debug.WriteLine("Animation: " + truckAnimation.Position.X + ", " + truckAnimation.Position.Y);

            // Debug.WriteLine(this.Position.X + ", " + this.Position.Y);

            truckAnimation.Update(gameTime);
        }

        public int Width
        {
            get { return truckAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return truckAnimation.FrameHeight; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; truckAnimation.Position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; truckAnimation.Position.Y = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; truckAnimation.Position = value; }
        }

        public float Rotation
        {
            get { return truckAnimation.angle; }
            set { truckAnimation.angle = value; }
        }

        
    }
}
