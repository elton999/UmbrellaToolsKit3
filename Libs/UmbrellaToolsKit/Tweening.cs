using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class Tweening
    {
        public static float Lerp(float min, float max, float value) => min + (max - min) * value;

        public static Vector2 Lerp(Vector2 min, Vector2 max, float value) => min + (max - min) * value;

        public static float LinearTween(float b, float c, float time, float duration) => c * time / duration + b;

        public static float EaseInQuad(float b, float c, float time, float duration) => c * (time /= duration) * time + b;

        public static float EaseOutQuad(float b, float c, float time, float duration) => -c * (time /= duration) * (time - 2) + b;
    }
}