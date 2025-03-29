using System;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public static class MathUtils
    {
        public static Vector2 Rotate(Vector2 vector, float angle)
        {
            angle = MathHelper.ToRadians(angle);

            float x = vector.X * (float)Math.Cos(angle) - vector.Y * (float)Math.Cos(angle);
            float y = vector.X * (float)Math.Sin(angle) + vector.Y * (float)Math.Cos(angle);

            vector = new Vector2(x, y);

            return vector;
        }

        public static float MillisecondsToSeconds(float milliSeconds) => milliSeconds / 1000.0f;
        public static float SecondsToMilliseconds(float seconds) => seconds * 1000.0f;

        public static float Divide(float value) => value / 2.0f;
        public static Vector2 Divide(Vector2 value) => value / 2.0f;
        public static Vector3 Divide(Vector3 value) => value / 2.0f;

        public static Vector2 TruncateVector(Vector2 value) => value.ToPoint().ToVector2();
    }
}