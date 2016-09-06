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
        public bool isInvincible = true;

        public static Texture2D truckTexture;
        public static Texture2D damagedTruck;

        public static int CrashedTrucks = 0;
        public float crashedTruckSpeedIncrease = 5f;

        public Animation truckAnimation;

        public bool active;
        public int health;

        public bool justSpawned = true;

        public Game1 activeGame;
        public bool isDestroyed = false;

        public float truckMoveSpeed = 400.0f;
        public float truckTauntedMoveSpeed = 600f;
        public float rotation;

        public Vector2 startPosition;
        public Vector2 targetPosition;

        public bool isTaunted = false;

        public Truck(Game1 game, Vector2 position)
        {
            // Debug.WriteLine(truckTexture == null);
            if (truckTexture == null)
            {
                truckTexture = game.Content.Load<Texture2D>("truck_sheet");
                damagedTruck = game.Content.Load<Texture2D>("monster_truck_damaged");
            }

            truckAnimation = new Animation();
            truckAnimation.Initialize(truckTexture, Vector2.Zero, 111, 92, 1, 1000, Color.White, 1f, true);
            truckAnimation.depth = 0.3f;
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
            // Debug.WriteLine("Collided");
            if (!isInvincible || !isDestroyed)
            {
                if (collidedWith.tag == "Truck")
                {
                    Destroy();

                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            truckAnimation.Draw(spriteBatch);
        }



        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            

            if (!isDestroyed)
            {
                if (isTaunted)
                {
                    truckAnimation.color = Color.Red;
                }


                if (!isInvincible)
                {
                    // Ram into the wall here
                    if (this.X - truckAnimation.FrameWidth / 2 < 0 || this.X > activeGame.GraphicsDevice.Viewport.Width || this.Y - truckAnimation.FrameWidth / 2 < 0 || this.Y > activeGame.GraphicsDevice.Viewport.Height)
                    {
                        Destroy();

                    }
                }


                Vector2 normalized = Vector2.Normalize(targetPosition - startPosition);
                if (!isTaunted)
                {
                    normalized *= (truckMoveSpeed + Truck.CrashedTrucks * crashedTruckSpeedIncrease);
                    this.X += normalized.X * deltaTime;
                    this.Y += normalized.Y * deltaTime;
                }
                else
                {
                    normalized *= (truckTauntedMoveSpeed + Truck.CrashedTrucks * crashedTruckSpeedIncrease);
                    this.X += normalized.X * deltaTime;
                    this.Y += normalized.Y * deltaTime;
                }

                if (this.X > 0 + truckAnimation.FrameWidth / 2 && this.X < activeGame.GraphicsDevice.Viewport.Width && this.Y > 0 + truckAnimation.FrameWidth / 2 && this.Y < activeGame.GraphicsDevice.Viewport.Height - 0)
                {
                    // Truck is in bounds
                    isInvincible = false;
                }

            }
            else
            {

            }


            // Debug.WriteLine("Animation: " + truckAnimation.Position.X + ", " + truckAnimation.Position.Y);

            // Debug.WriteLine(this.Position.X + ", " + this.Position.Y);

            truckAnimation.Update(gameTime);
        }

        public void Taunt()
        {
            this.targetPosition = activeGame.player.Position;
            this.startPosition = this.Position;
            this.Rotation = (float)(3 * Math.PI / 2 + activeGame.VectorToAngle(this.startPosition - this.targetPosition));
            isTaunted = true;

        }

        public void Destroy()
        {
            Debug.WriteLine("is destroyed");
            isDestroyed = true;
            // Truck is destroyed
            truckAnimation = new Animation();
            truckAnimation.Initialize(damagedTruck, this.Position, 111, 92, 1, 1000, new Color(0.5f, 0.5f, 0.5f, 1.0f), 1f, true);
            truckAnimation.angle = Rotation + (float)Math.PI;
            truckAnimation.depth = 0.5f;
            Truck.CrashedTrucks++;
        }

        public bool IsCurrentlyCollideable
        {
            get
            {
                if (isDestroyed || isInvincible) return false;
                else return true;
            }
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
            get { return rotation; }
            set { rotation = value;  truckAnimation.angle = value; }
        }

        
    }
}
