using System;

namespace UmbrellaToolsKit.Utils
{
    public class Timer
    {
        private DateTime startDateTime;
        private DateTime endDateTime;

        public void Begin() => startDateTime = DateTime.Now;

        public void End() => endDateTime = DateTime.Now;

        public float GetTotalSeconds() => (float)(endDateTime - startDateTime).TotalSeconds;
    }
}
