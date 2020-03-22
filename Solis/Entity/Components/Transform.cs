using Microsoft.Xna.Framework;

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