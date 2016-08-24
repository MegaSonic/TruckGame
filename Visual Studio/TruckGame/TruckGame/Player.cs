using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    class Player
    {
        // public Texture2D PlayerTexture;
        public Animation playerAnimation;

        public Vector2 position;
        public bool active;
        public int health;

        public void Initialize(Animation animation, Vector2 position)
        {
            playerAnimation = animation;
            this.position = position;
            active = true;
            health = 100;
        }

        public void Update(GameTime gameTime)
        {
            playerAnimation.Position = position;
            playerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
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

        public float Angle
        {
            get { return playerAnimation.angle; }
            set { playerAnimation.angle = value; }
        }
    }
}