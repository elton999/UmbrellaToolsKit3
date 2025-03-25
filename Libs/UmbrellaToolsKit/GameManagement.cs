using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine;
#if !RELEASE
using Eto;
using Eto.Forms;
#endif

namespace UmbrellaToolsKit
{
    public class GameManagement
    {
        private EditorMain _editor;

        public Dictionary<String, dynamic> Values = new Dictionary<string, dynamic>();

        public enum Status { LOADING, CREDITS, MENU, PAUSE, STOP, PLAYING };
        public Status CurrentStatus;

        public enum GameplayStatus { ALIVE, DEATH, };
        public GameplayStatus CurrentGameplayStatus;

        public SceneManagement SceneManagement;
        public SpriteBatch SpriteBatch;
        public Game Game;

        public static event Action OnGameUpdateData;

        private EditorMain _edtior;

        public GameManagement(Game game)
        {
            Game = game;
            _editor = new EditorMain(Game, this);
#if !RELEASE
            new Application(new Eto.WinForms.Platform());
#endif
        }

        public void Start()
        {
            CurrentStatus = Status.PLAYING;
            SceneManagement = new SceneManagement();
            SceneManagement.GameManagement = this;
            SceneManagement.Start();
            SceneManagement.MainScene.CreateBackBuffer();
        }

        public void Update(GameTime gameTime)
        {
            SceneManagement.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteBatch = spriteBatch;
            SceneManagement.Draw(spriteBatch);
            _editor.Draw(gameTime);
            OnGameUpdateData?.Invoke();
        }
    }
}
