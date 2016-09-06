using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace TruckGame
{
    public class Button : GameObject
    {
        MouseState currentMouseState;
        
        

        public Texture2D texture;
        Vector2 ButtonPosition = new Vector2() ;

      
        

        public string Name { get; private set; }
        
        public Button(string name, Vector2 position)
        {
            this.Name = name;
            this.position = position;
        }
        public bool enterButton(MouseState currentMouseState)
        {
            if (currentMouseState.X < (position.X + texture.Width/2) &&
                   currentMouseState.X > (position.X - texture.Width/2) &&
                   currentMouseState.Y < (position.Y + texture.Height/2) && // lower bound
                   currentMouseState.Y > (position.Y - texture.Height/2)) // upper bound
            {
                // Console.WriteLine(currentMouseState.X + "True" );
                return true;
            }
            // Console.WriteLine(currentMouseState.X + "False" + texture.Width + texture.Height);
            //Console.WriteLine()
            // // return false; 
            return false;   

        }

        float radius=200;
        public bool isInsideCircle(MouseState currentMouseState)
        {
            Console.WriteLine("Checking if inside circle");
            if (Vector2.Distance(currentMouseState.Position.ToVector2(), position) < radius)
            {
                Console.WriteLine("Mouse Inside Circle");
                return true;
            }
            return false;

        }



    }
}
