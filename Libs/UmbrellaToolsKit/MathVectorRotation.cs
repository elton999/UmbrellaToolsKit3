using System;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public static class MathVectorRotation
    {
        public static Vector2 Rotate(Vector2 vector, float angle)
        {
            angle = MathHelper.ToRadians(angle);

            float x = vector.X * (float)Math.Cos(angle) - vector.Y * (float)Math.Cos(angle);
            float y = vector.X * (float)Math.Sin(angle) + vector.Y * (float)Math.Cos(angle);

            vector = new Vector2(x, y);

            return vector;
        }
    }
}