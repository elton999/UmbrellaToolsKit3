using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit
{
    public class SceneManagement : Interfaces.IDrawable
    {
        public GameManagement GameManagement;
        public Scene MainScene;
        public int CurrentScene = 1;
        public int MaxScenes = 1;

        public virtual void Start() => SetScene(CurrentScene);

        public virtual void SetScene(int Scene)
        {
            MainScene = new Scene(
                GameManagement.Game.GraphicsDevice,
                GameManagement.Game.Content
            );
            MainScene.GameManagement = GameManagement;
        }

        public void Update(GameTime gameTime)
        {
            if (MainScene != null && MainScene.LevelReady)
                MainScene.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (MainScene != null && MainScene.LevelReady)
                MainScene.Draw(spriteBatch,
                GameManagement.Game.GraphicsDevice,
                new Vector2(
                    GameManagement.Game.GraphicsDevice.Viewport.Width,
                    GameManagement.Game.GraphicsDevice.Viewport.Height
                ));
        }
    }
}