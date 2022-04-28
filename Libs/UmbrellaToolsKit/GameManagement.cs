using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit
{
    public class GameManagement : IUpdatable, Interfaces.IDrawable
    {
        public Dictionary<String, dynamic> Values = new Dictionary<string, dynamic>();

        public enum Status { LOADING, CREDITS, MENU, PAUSE, STOP, PLAYING };
        public Status CurrentStatus;

        public enum GameplayStatus { ALIVE, DEATH, };
        public GameplayStatus CurrentGameplayStatus;

        public SceneManagement SceneManagement;
        public Game Game;

        public void Start()
        {
            this.CurrentStatus = Status.PLAYING;
            this.SceneManagement = new SceneManagement();
            this.SceneManagement.GameManagement = this;
        }

        public void Update(GameTime gameTime) => SceneManagement.Update(gameTime);

        public void Draw(SpriteBatch spriteBatch) => SceneManagement.Draw(spriteBatch);
    }
}
