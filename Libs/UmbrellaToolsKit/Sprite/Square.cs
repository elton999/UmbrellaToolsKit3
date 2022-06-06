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

            this.Sprite = new Texture2D(this.Scene.ScreenGraphicsDevice, this.size.X, this.size.Y);
            Color[] data = new Color[this.size.X * this.size.Y];
            for (int i = 0; i < data.Length; ++i)
                data[i] = this.SquareColor;
            this.Sprite.SetData(data);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch);
            DrawSprite(spriteBatch);
            EndDraw(spriteBatch);
        }
    }
}
