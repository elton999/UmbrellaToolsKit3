using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Sprite
{
    public class AsepriteAnimation
    {
        public Rectangle Body { get; set; }
        private AsepriteDefinitions AsepriteDefinitions { get; set; }

        public AsepriteAnimation(AsepriteDefinitions AsepriteDefinitions)
        {
            this.AsepriteDefinitions = AsepriteDefinitions;
        }

        #region play animation
        public int frame;
        public int frameCurrent;
        private float frameTimerCount;
        private List<float> maxFrame;
        private AnimationDirection direction;
        public enum AnimationDirection { FORWARD, LOOP, PING_PONG }
        private bool checkedFirstframe;

        private int a_from;
        private int a_to;
        public string tag;

        public bool lastFrame
        {
            get
            {
                if (this.maxFrame != null)
                {
                    if (this.maxFrame[this.maxFrame.Count - 1] < this.frameTimerCount && this.tag != null) return true;
                }
                return false;
            }
        }

        public void Restart()
        {
            this.tag = "";
        }

        public void Play(GameTime gameTime, string tag, AnimationDirection aDirection = AnimationDirection.FORWARD)
        {
            if (tag != this.tag)
            {
                int i = 0;
                while (i < this.AsepriteDefinitions.Tags.Count)
                {
                    if (tag == this.AsepriteDefinitions.Tags[i].Name)
                    {
                        this.a_from = this.AsepriteDefinitions.Tags[i].from;
                        this.a_to = this.AsepriteDefinitions.Tags[i].to;
                        this.tag = this.AsepriteDefinitions.Tags[i].Name;
                        this.direction = aDirection;
                        this.frameCurrent = 0;
                        this.frameTimerCount = 0;
                        this.maxFrame = new List<float>();
                        this.checkedFirstframe = false;
                        break;
                    }
                    i++;
                }

                i = 0;
                while (i + this.a_from <= this.a_to)
                {
                    int i_frame = this.a_from + i;
                    int last_frame = i - 1;

                    if (i > 0) this.maxFrame.Add((this.AsepriteDefinitions.Duration[i_frame]) + this.maxFrame[last_frame]);
                    else this.maxFrame.Add(this.AsepriteDefinitions.Duration[i_frame]);

                    i++;
                }
            }

            float delta = (float)gameTime.ElapsedGameTime.Milliseconds;
            this.frameTimerCount += delta;

            if (this.frameTimerCount >= this.maxFrame[this.frameCurrent] || (this.frameCurrent == 0 && !checkedFirstframe))
            {
                if (this.a_to > (this.frameCurrent + this.a_from) && checkedFirstframe) this.frameCurrent++;
                else if (this.direction == AnimationDirection.LOOP)
                {
                    this.frameCurrent = 0;
                    this.frameTimerCount = 0;
                    this.checkedFirstframe = false;
                }

                frame = (int)(this.frameCurrent + this.a_from);
                this.Body = this.AsepriteDefinitions.Bodys[frame];

                this.checkedFirstframe = true;
            }

        }
        #endregion
    }
}
