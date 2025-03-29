using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Sprite
{
    public class AsepriteAnimation : ISpriteAnimation
    {
        public Rectangle Body { get; set; }
        public AsepriteDefinitions AsepriteDefinitions { get; set; }

        public int Frame { get; set; }
        public int CurrentFrame { get; set; }
        private float frameTimerCount;
        private List<float> MaxFrame = new List<float>();
        private AnimationDirection direction;
        private bool isTheFirstFrame;

        private int a_from;
        private int a_to;
        public string currentAnimationName { get; set; }

        public bool IsTheLasFrame
        {
            get
            {
                bool hasValidFrames = !String.IsNullOrEmpty(currentAnimationName) && MaxFrame.Count > 0;
                if (hasValidFrames)
                    if (MathUtils.MillisecondsToSeconds(MaxFrame[MaxFrame.Count - 1]) < frameTimerCount) return true;
                return false;
            }
        }

        public AsepriteAnimation(AsepriteDefinitions asepriteDefinitions) => AsepriteDefinitions = asepriteDefinitions;

        public void Restart() => currentAnimationName = String.Empty;

        public void Play(float deltaTime, string animationName, AnimationDirection aDirection = AnimationDirection.FORWARD)
        {
            if (animationName != currentAnimationName)
            {
                SetAnimation(animationName, aDirection);
                SetAllFrameOfAnimation();
            }

            frameTimerCount += deltaTime;
            bool reachMaxTimeOfFrame = frameTimerCount >= MathUtils.MillisecondsToSeconds(MaxFrame[CurrentFrame]);
            bool hasJustStartedTheAnimation = CurrentFrame == 0 && !isTheFirstFrame;
            bool CanGoToNextFrame = reachMaxTimeOfFrame || hasJustStartedTheAnimation;

            if (CanGoToNextFrame) NextFrame();
        }

        public void SetAllFrameOfAnimation()
        {
            for (int frameCount = 0; frameCount + a_from <= a_to; frameCount++)
            {
                int frameIndex = a_from + frameCount;
                int last_frame = frameCount - 1;

                var durationFrames = AsepriteDefinitions.Duration;
                float currentDuration = (float)durationFrames[frameIndex];
                if (frameCount > 0) MaxFrame.Add(currentDuration + MaxFrame[last_frame]);
                else MaxFrame.Add(currentDuration);
            }
        }

        public void SetAnimation(string animationName, AnimationDirection aDirection)
        {
            int i = 0;
            while (i < AsepriteDefinitions.Tags.Count)
            {
                var animationData = AsepriteDefinitions.Tags[i];
                if (animationName == animationData.Name)
                {
                    a_from = animationData.from;
                    a_to = animationData.to;
                    currentAnimationName = animationData.Name;
                    direction = aDirection;
                    CurrentFrame = 0;
                    frameTimerCount = 0.0f;
                    MaxFrame = new List<float>();
                    isTheFirstFrame = false;
                    break;
                }
                i++;
            }
        }

        public void NextFrame()
        {
            if (a_to > (CurrentFrame + a_from) && isTheFirstFrame) CurrentFrame++;
            else if (direction == AnimationDirection.LOOP)
            {
                CurrentFrame = 0;
                frameTimerCount = 0.0f;
                isTheFirstFrame = false;
            }

            Frame = (int)(CurrentFrame + a_from);
            Body = AsepriteDefinitions.Bodys[Frame];

            isTheFirstFrame = true;
        }
    }
}
