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
        int buttonX, buttonY;
        public Texture2D texture;
        Vector2 ButtonPosition = new Vector2() ;

        public int ButtonX
        {
            get
            {
                return buttonX;
            }
        }
       
        public int ButtonY
        {
            get
            {
                return ButtonY;
            }
        }

        public string Name { get; private set; }
        public Texture2D Texture { get; private set; }

        public Button(string name, int buttonX, int buttonY)
        {
            this.Name = name;
           
            this.buttonX = buttonX;
            this.buttonY = buttonY;
        }
        public bool enterButton()
        {
            if ( Mouse.GetState().X < ButtonPosition.X + Texture.Width &&
                   Mouse.GetState().X > ButtonPosition.X &&
                   Mouse.GetState().Y < ButtonPosition.Y + Texture.Height &&
                   Mouse.GetState().Y > ButtonPosition.Y)
            {
                 return true;
            }

            // // return false; 
            return false;   

        }

    }
}
