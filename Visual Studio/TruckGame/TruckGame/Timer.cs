using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TruckGame
{
    /// <summary>
    /// Timer class that displays minutes, seconds, and centiseconds
    /// </summary>
    public class Timer : GameObject
    {
        public SpriteFont font;
        public float playerTime = 0f;
        string formattedTime;
        public int points = 0;


        public Timer(Game1 game)
        {
            font = game.Content.Load<SpriteFont>("Timer");
            tag = "Timer";
            position = new Vector2(10, 10);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(font, formattedTime, position, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            points += (int) (100 * deltaTime);

            formattedTime = points.ToString();
        }
    }
}
