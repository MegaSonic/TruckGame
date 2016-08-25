using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame 
{
    class Sprite : GameObject
    {
        Texture2D texture;

        public Sprite(Texture2D _texture, Vector2 _position) : base(_position)
        {
            texture = _texture;
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X - texture.Width / 2,
                    (int)Position.Y - texture.Height / 2,
                    texture.Width,
                    texture.Height);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, depth);
        }

        public override void Update()
        {
            
        }
    }
}
