using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Sprite
{
    public enum AnimationDirection { FORWARD, LOOP, PING_PONG }

    public interface ISpriteAnimation
    {
        public Rectangle Body { get; set; }

        public int Frame { get; set; }
        public int CurrentFrame { get; set; }
        public string currentAnimationName { get; set; }
        public bool IsTheLasFrame { get; }

        public void Restart();
        public void Play(float deltaTime, string animationName, AnimationDirection aDirection = AnimationDirection.FORWARD);
        public void NextFrame();
    }
}