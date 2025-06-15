using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Sprite
{
    public class Square : GameObject
    {
        public Color SquareColor;
        public override void Start()
        {
            base.Start();

            /*Sprite = new Texture2D(Scene.ScreenGraphicsDevice, size.X, size.Y);
            Color[] data = new Color[size.X * size.Y];
            for (int i = 0; i < data.Length; ++i)
                data[i] = SquareColor;
            Sprite.SetData(data);*/
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch);
            DrawSprite(spriteBatch);
            EndDraw(spriteBatch);
        }

        public override void Dispose()
        {
            Sprite.Dispose();
            base.Dispose();
        }
    }
}
