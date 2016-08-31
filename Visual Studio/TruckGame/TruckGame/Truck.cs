using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    public class Truck : GameObject, ICollideable
    {
        public bool isInvincible;

        public Animation truckAnimation;

        public bool active;
        public int health;

        public Game1 activeGame;

        public float truckMoveSpeed = 150.0f;
        public float truckTurnSpeed = 1.0f;



        public Truck(Game1 game, Animation animation, Vector2 position)
        {
            activeGame = game;
            truckAnimation = animation;
            this.position = position;
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
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
