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

        public GameManagement(Game game) => Game = game;

        public void Start()
        {
            CurrentStatus = Status.PLAYING;
            SceneManagement = new SceneManagement();
            SceneManagement.GameManagement = this;
            SceneManagement.Start();
            SceneManagement.MainScene.CreateBackBuffer();
        }

        public void Update(GameTime gameTime) => SceneManagement.Update(gameTime);

        public void Draw(SpriteBatch spriteBatch) => SceneManagement.Draw(spriteBatch);
    }
}
