using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis
{
    public class Transform : Component
    {
        public Vector2 Position;

        public Transform(Vector2 startPosition)
        {
            Position = startPosition;
        }
    }
}
