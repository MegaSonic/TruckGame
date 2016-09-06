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


        public Timer(Game1 game)
        {
            font = game.Content.Load<SpriteFont>("Timer");
            tag = "Timer";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, formattedTime, new Vector2(10, 10), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            playerTime += deltaTime;

            // Gets the minute aspect of player time
            int minuteInt = (int)playerTime / 60;
            string minutes;
            if (minuteInt < 10)
                minutes = "0" + minuteInt.ToString();
            else
                minutes = minuteInt.ToString();


            // Gets the second aspect of player time
            int secondInt = (int) Math.Floor(playerTime % 60);
            string seconds;
            if (secondInt < 10)
                seconds = "0" + secondInt.ToString();
            else
                seconds = secondInt.ToString();

            // Gets the millisecond aspect of player time
            int millisecondInt = (int) Math.Floor((playerTime % 1f) * 100f);
            string milliseconds;
            if (millisecondInt < 10)
                milliseconds = "0" + millisecondInt.ToString();
            else
                milliseconds = millisecondInt.ToString();

            formattedTime = (minutes + "'" + seconds + "'" + milliseconds);
        }
    }
}
