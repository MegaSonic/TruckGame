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
        public static Texture2D driveTruck;
        public static Texture2D crashTruck;

        public static float ActiveTruckDepth = 0.3f;
        public static float CrashedTruckDepth = 0.5f;

        public static int CrashedTrucks = 0;
        public float crashedTruckSpeedIncrease = 5f;

        public Animation truckAnimation = new Animation();
        public Animation deadAnimation = new Animation();
        public Animation crashAnimation = new Animation();
        public Animation driveAnimation = new Animation();

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

        public float radius = 50f;

        public bool displayingPoints = false;
        public int pointValue;
        public float pointDisplayTime = 2f;
        private float pointTimer = 0f;
        public Color pointColor = new Color(0, 255, 0);
        private bool changingToWhite = true;


        public bool isTaunted = false;

        public Truck(Game1 game, Vector2 position)
        {
            // Debug.WriteLine(truckTexture == null);
            if (truckTexture == null)
            {
                truckTexture = game.Content.Load<Texture2D>("truck_sheet");
                damagedTruck = game.Content.Load<Texture2D>("monster_truck_damaged");
                crashTruck = game.Content.Load<Texture2D>("truck_crash");
                driveTruck = game.Content.Load<Texture2D>("truck_drive");
            }

            pointTimer = pointDisplayTime;
            
            deadAnimation.Initialize(damagedTruck, Vector2.Zero, 170, 103, 1, 1000, new Color(0.5f, 0.5f, 0.5f, 1.0f), 1f, true);
            deadAnimation.depth = 0.3f;
            deadAnimation.pivot = new Vector2(105, 53);

            crashAnimation.Initialize(crashTruck, Vector2.Zero, 170, 103, 8, 80, Color.White, 1f, false);
            crashAnimation.depth = 0.3f;
            crashAnimation.pivot = new Vector2(105, 53);

            driveAnimation.Initialize(driveTruck, Vector2.Zero, 170, 103, 4, 80, Color.White, 1f, true);
            driveAnimation.depth = 0.3f;
            driveAnimation.pivot = new Vector2(105, 53);

            truckAnimation = driveAnimation;

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
            if (!displayingPoints)
            {
                truckAnimation.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.DrawString(activeGame.timer.font, "+" + pointValue, this.Position, pointColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.2f);
            }
        }



        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            truckAnimation.Position = this.Position;

            if (displayingPoints)
            {
                
                pointTimer -= deltaTime;
                if (pointTimer < 0f)
                {
                    activeGame.objectsToRemove.Add(this);
                }

                if (changingToWhite)
                {
                    pointColor = new Color(pointColor.ToVector3().X + 3 * deltaTime, 1f, pointColor.ToVector3().Z + 3 * deltaTime);
                    if (pointColor.ToVector3().X >= 1f)
                    {
                        changingToWhite = false;
                    }
                }
                else
                {
                    pointColor = new Color(pointColor.ToVector3().X - 3 * deltaTime, 1f, pointColor.ToVector3().Z - 3 * deltaTime);
                    if (pointColor.ToVector3().X <= 0f)
                    {
                        changingToWhite = true;
                    }
                }
            }

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
                if (truckAnimation == crashAnimation)
                {
                    truckAnimation.color = new Color(truckAnimation.color.ToVector3().X - 0.5f * deltaTime, truckAnimation.color.ToVector3().Y - 0.5f * deltaTime, truckAnimation.color.ToVector3().Z - 0.5f * deltaTime);
                }
            }

            truckAnimation.Update(gameTime);

            // Change the truck animation from the crashing to destroyed
            if (truckAnimation == crashAnimation && !truckAnimation.Active)
            {
                if (!truckAnimation.Active)
                {
                    // Debug.WriteLine("Not active anymore!");
                    deadAnimation.depth = truckAnimation.depth;
                    deadAnimation.Position = this.Position;
                    deadAnimation.angle = this.Rotation;
                    deadAnimation.color = truckAnimation.color;
                    truckAnimation = deadAnimation;
                    truckAnimation.Update(gameTime);
                }
            }
        }

        public void Taunt()
        {
            this.targetPosition = activeGame.player.Position;
            this.startPosition = this.Position;
            this.Rotation = (float)(1 * Math.PI / 2 + activeGame.VectorToAngle(this.startPosition - this.targetPosition));
            isTaunted = true;

        }

        public void Destroy()
        {
            // Debug.WriteLine("is destroyed");
            isDestroyed = true;
            
            // Truck is destroyed
            truckAnimation = crashAnimation;
            truckAnimation.angle = this.Rotation;
            truckAnimation.depth = CrashedTruckDepth;
            CrashedTruckDepth -= 0.0001f;
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

        public float Radius
        {
            get
            {
                return radius;
            }
        }
    }
}
