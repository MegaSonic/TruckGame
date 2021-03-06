﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    public class GameObject
    {
        
        public Vector2 position;
        public float rotation = 0.0f;
        public float depth = 1.0f;
        public float scale = 1.0f;

        public string tag;

        public GameObject()
        {

        }

        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public virtual void Start()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(texture, Position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, depth);
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
