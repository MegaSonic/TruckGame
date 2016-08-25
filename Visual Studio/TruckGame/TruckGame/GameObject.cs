using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    class GameObject
    {
        
        public Vector2 Position;
        public float rotation = 0.0f;
        public float depth = 1.0f;
        public float scale = 1.0f;


        public GameObject(Vector2 position)
        {
            this.Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(texture, Position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, depth);
        }

        public virtual void Update()
        {
            
        }
    }
}
