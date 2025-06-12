using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Components.Physics;

namespace UmbrellaToolsKit.Utils
{
    public static class Collision
    {
        public static bool OverlapCheck(Point size1, Vector2 position1, Point size2, Vector2 position2)
        {
            bool AisToTheRightOfB = position1.X >= position2.X + size2.X;
            bool AisToTheLeftOfB = position1.X + size1.X <= position2.X;
            bool AisAboveB = position1.Y + size1.Y <= position2.Y;
            bool AisBelowB = position1.Y >= position2.Y + size2.Y;
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        public static bool OverlapCheckPixel(ActorComponent actor1, ActorComponent actor2)
        {
            bool AisToTheRightOfB = actor1.Left > actor2.Right;
            bool AisToTheLeftOfB = actor1.Right < actor2.Left;
            bool AisAboveB = actor1.Bottom < actor2.Top;
            bool AisBelowB = actor1.Top > actor2.Bottom;
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }
    }
}