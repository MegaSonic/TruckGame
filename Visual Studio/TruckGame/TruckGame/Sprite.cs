using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TruckGame 
{
    class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public Color color;
        public float rotation, depth, scale;

        public Sprite(Texture2D _texture, Vector2 _position, ContentManager _content, string asset)
        {
            texture = _texture;
            position = _position;
            texture = _content.Load<Texture2D>(asset);
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)position.X - texture.Width / 2,
                    (int)position.Y - texture.Height / 2,
                    texture.Width,
                    texture.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, depth);
        }
    }
}
