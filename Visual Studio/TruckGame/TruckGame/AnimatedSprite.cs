using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TruckGame
{
    class AnimatedSprite
    {
        public Texture2D texture;
        public Vector2 position;

        private int framecount;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;

        public Color color = Color.White;

        public AnimatedSprite(Vector2 _position, Texture2D _texture)
        {
            texture = _texture;
            position = _position;
        }

        public void Load(ContentManager content, string asset, int frameCount, int framesPerSec)
        {
            framecount = frameCount;
            texture = content.Load<Texture2D>(asset);
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        // class AnimatedTexture
        public void Draw(SpriteBatch batch, Vector2 screenPos)
        {
            Draw(batch, Frame, screenPos);
        }

        public void Draw(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = texture.Width / framecount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, texture.Height);
            batch.Draw(texture, screenPos, sourcerect, color, Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }

        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            Paused = false;
        }

        public void Pause()
        {
            Paused = true;
        }

    }
}
