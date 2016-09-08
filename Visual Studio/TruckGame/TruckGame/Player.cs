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
using Microsoft.Xna.Framework.Audio;



namespace TruckGame
{
    public class Player : GameObject, ICollideable
    {
        // public Texture2D PlayerTexture;
        public Animation playerAnimation;

        public Texture2D dashSheet;
        public Animation dashAnimation;
        public Animation walkAnimation;

        public bool active;
        public int health;

        public Game1 activeGame;

        public float playerMoveSpeed = 300.0f;
        public float playerTurnSpeed = 1.0f;

        private float dodgeTimer = 0f;
        private float dodgeCooldownTimer = 0f;
        public float dodgeLength = 0.2f;
        public float dodgeCooldownLength = 0.3f;
        public float dodgeSpeed = 1000.0f;

        private float tauntTimer = 0f;
        public float tauntCooldownLength = 1f;
        public float tauntRadius = 450f;

        public bool isDodgeRolling = false;

        public float radius = 15f;
        private static SoundEffect dash;
        private static SoundEffect tauntfx;

        public Player()
        {
            
        }

        public void Start(Game1 game, Animation animation, Vector2 position)
        {
            dash = game.Content.Load<SoundEffect>("Dash");
            tauntfx = game.Content.Load<SoundEffect>("Taunt");

            animation.pivot = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);
            playerAnimation = animation;
            walkAnimation = animation;
            dashSheet = game.Content.Load<Texture2D>("player_dash");
            dashAnimation = new Animation();
            dashAnimation.Initialize(dashSheet, position, 110, 60, 2, 100, Color.White, 1.5f, true);
            
            dashAnimation.pivot = new Vector2(dashAnimation.FrameWidth / 2, dashAnimation.FrameHeight / 2);
            
            this.Position = position;
            dashAnimation.Position = this.Position;
            active = true;
            health = 100;
            activeGame = game;
            tag = "Player";
            playerAnimation.depth = 0.1f;
            dashAnimation.depth = playerAnimation.depth;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (dodgeTimer > 0f) dodgeTimer -= deltaTime;
            if (dodgeCooldownTimer > 0f && !isDodgeRolling) dodgeCooldownTimer -= deltaTime;
            if (tauntTimer > 0f) tauntTimer -= deltaTime;
            if (dodgeTimer < 0f)
            {
                isDodgeRolling = false;
                playerAnimation = walkAnimation;
            }


            // this.X += activeGame.currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed * deltaTime;
            // this.Y -= activeGame.currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed * deltaTime;


            playerAnimation.Position = this.Position;

            bool left = false, right = false;
            bool anyDirection = false;

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Left) || activeGame.currentGamePadState.DPad.Left == ButtonState.Pressed)
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
                anyDirection = true;
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Right) || activeGame.currentGamePadState.DPad.Right == ButtonState.Pressed)
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
                anyDirection = true;
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Up) || activeGame.currentGamePadState.DPad.Up == ButtonState.Pressed)
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
                anyDirection = true;
            }

            if (activeGame.currentKeyboardState.IsKeyDown(Keys.Down) || activeGame.currentGamePadState.DPad.Down == ButtonState.Pressed)
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
                anyDirection = true;
            }

            // Dodge Roll
            if (((activeGame.currentKeyboardState.IsKeyDown(Keys.Z) && activeGame.previousKeyboardState.IsKeyUp(Keys.Z)) || (activeGame.currentKeyboardState.IsKeyDown(Keys.Space) && activeGame.previousKeyboardState.IsKeyUp(Keys.Space))) && dodgeCooldownTimer <= 0 && anyDirection)
            {
                dash.Play();
                Debug.WriteLine("Dodge");
                isDodgeRolling = true;
                dodgeTimer = dodgeLength;
                dodgeCooldownTimer = dodgeCooldownLength;
                playerAnimation = dashAnimation;
                
            }

            // Taunt
            if (((activeGame.currentKeyboardState.IsKeyDown(Keys.X) && activeGame.previousKeyboardState.IsKeyUp(Keys.X)) || (activeGame.currentKeyboardState.IsKeyDown(Keys.LeftShift) && activeGame.previousKeyboardState.IsKeyUp(Keys.LeftShift))) && tauntTimer <= 0)
            {
                tauntfx.Play();
                Shockwave taunt = new Shockwave(this.Position, activeGame);
                activeGame.objectsToAdd.Add(taunt);
                tauntTimer = tauntCooldownLength;
                int points = 100;
                foreach (GameObject go in activeGame.FindGameObjectsByTag("Truck"))
                {
                    Truck truck = go as Truck;
                    if (truck.isDestroyed)
                    {
                        if (Math.Pow(truck.X - this.X, 2) + Math.Pow(truck.Y - this.Y, 2) < Math.Pow(tauntRadius, 2))
                        {
                            // activeGame.objectsToRemove.Add(go);

                            truck.displayingPoints = true;
                            truck.pointValue = points;
                            if (points < 25600)
                            {
                                points *= 2;
                            }
                            activeGame.timer.points += points;
                            Truck.CrashedTrucks--;
                        }
                    }
                    else
                    {
                        truck.Taunt();
                    }
                }

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
            if (collidedWith.tag == "Truck")
            {
                Bloodstain stain = new Bloodstain(this.Position, (float) ((1 * Math.PI / 2) * activeGame.VectorToAngle(collidedWith.position - this.Position)), activeGame);

                /*
                Console.WriteLine("Player at " + this.Position.X + ", " + this.Position.Y);
                Console.WriteLine("Truck at " + collidedWith.position.X + ", " + collidedWith.position.Y);
                float distanceBetweenObjects = (float)(Math.Pow(this.position.X - collidedWith.position.X, 2) + Math.Pow(this.position.Y - collidedWith.position.Y, 2));
                ICollideable secondCollideable = collidedWith as ICollideable;
                float sumOfRadii = (float)(Math.Pow(this.Radius + secondCollideable.Radius, 2));

                Console.WriteLine("Player's pivot: " + this.Pivot);
                Console.WriteLine("Truck's pivot: " + secondCollideable.Pivot);
                Console.WriteLine("Distance squared: " + distanceBetweenObjects);
                Console.WriteLine("Radius squared: " + sumOfRadii);
                */

                activeGame.objectsInScene.Add(stain);
                activeGame.Reset();


            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
        }

        public Vector2 Pivot
        {
            get
            {
                return playerAnimation.Position + walkAnimation.pivot;
            }
        }
    }
}