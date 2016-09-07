using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TruckGame
{
    public class Shockwave: GameObject
    {
        public Animation animation;
        public Texture2D texture;
        public Game1 activeGame;

        public Shockwave(Vector2 position, Game1 game)
        {
            if (texture == null)
            {
                texture = game.Content.Load<Texture2D>("shockwave_ole");
            }
            this.position = position;
            tag = "Taunt";
            activeGame = game;
            animation = new Animation();
            animation.Initialize(texture, position, 900, 900, 14, 10, Color.White, 1f, false);
            animation.pivot = new Vector2(0, 0);
            animation.Position = this.position;
            animation.depth = 0f;
            Debug.WriteLine("Shockwave created");
        }

        public override void Update(GameTime gameTime)
        {

            if (!animation.Active)
            {
                Debug.WriteLine("Removed");
                activeGame.objectsToRemove.Add(this);
            }

            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
