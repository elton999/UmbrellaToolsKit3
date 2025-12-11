using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Interfaces
{
    public interface ITexture
    {
        public Texture2D GetTexture();
        public void SetTexture(Texture2D texture);
    }
}
