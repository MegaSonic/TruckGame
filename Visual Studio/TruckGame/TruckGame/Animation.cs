using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    public class Animation
    {
        Texture2D spriteStrip;
        public float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;
        public float angle;

        public Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destinationRect = new Rectangle();
        public int FrameWidth;
        public int FrameHeight;
        public bool Active = true;
        public bool Looping;
        public Vector2 Position;
        public float depth = 1.0f;
        public Vector2 pivot;

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            // Keep a local copy of the values passed in

            this.color = color;

            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;

            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            pivot = new Vector2(FrameWidth / 2, FrameHeight / 2);

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;
            angle = 0.0f;

            // Set the Animation to active by default
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false) return;

            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;

                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;

            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect.X = (int)(Position.X - (FrameWidth * scale) / 2);
            destinationRect.Y = (int)(Position.Y - (FrameHeight * scale) / 2);

            destinationRect.Width = (int)(FrameWidth * scale);
            destinationRect.Height = (int)(FrameHeight * scale);
        }

        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                // spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, angle, pivot, SpriteEffects.None, depth);
                spriteBatch.Draw(spriteStrip, Position, sourceRect, color, angle, pivot, scale, SpriteEffects.None, depth);

            }
        }

    }
}
