using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbrellaToolKit
{
    public class GameManagement : GameObject
    {
        public Dictionary<String, dynamic> Values = new Dictionary<string, dynamic>();
        public enum Status  { LOADING, CREDITS, MENU, PAUSE, STOP, PLAYING};
        public Status CurrentStatus;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.processWait(gameTime);
        }
    }
}
