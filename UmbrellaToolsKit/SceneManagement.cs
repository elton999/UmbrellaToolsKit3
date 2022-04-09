using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit
{
    public class SceneManagement
    {
        public GameManagement GameManagement;
        public Scene MainScene;
        public int CurrentScene = 1;
        public int MaxScenes = 1;

        public virtual void Start() => SetScene(CurrentScene);

        public virtual void SetScene(int Scene)
        {
            this.MainScene = new Scene(
                GameManagement.Game.GraphicsDevice,
                GameManagement.Game.Content
            );
            this.MainScene.GameManagement = this.GameManagement;
            //this.MainScene.SetLevel(Scene);
            this.MainScene.LevelReady = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.MainScene != null && this.MainScene.LevelReady)
                this.MainScene.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.MainScene != null && this.MainScene.LevelReady)
                this.MainScene.Draw(spriteBatch,
                GameManagement.Game.GraphicsDevice,
                new Vector2(
                    GameManagement.Game.GraphicsDevice.Viewport.Width,
                    GameManagement.Game.GraphicsDevice.Viewport.Height
                ));
        }
    }
}