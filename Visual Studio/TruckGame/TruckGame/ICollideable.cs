using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace TruckGame
{
    public interface ICollideable
    {
        Rectangle BoundingBox
        {
            get;
        }

        void Collided(GameObject collidedWith);

    }
}
