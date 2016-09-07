using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    public class Bloodstain: GameObject
    {
        public static Texture2D texture;
        public Game1 activeGame; 



        public Bloodstain(Vector2 position, float rotation, Game1 game)
        {
            if (texture == null)
            {
                texture = game.Content.Load<Texture2D>("single_splat");
            }
            this.position = position;
            this.rotation = rotation;
            tag = "Bloodstain";
            activeGame = game;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(84, 180), 1f, SpriteEffects.None, 0.9f);
            // spriteBatch.Draw(texture, this.position, null, Color.White, rotation + (float) Math.PI, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0.9f);
        }
    }
}
