using Microsoft.Xna.Framework.Graphics;
using System;

namespace UmbrellaToolsKit.Interfaces
{
    public interface IDrawable
    {
        Action<SpriteBatch> ExtraDraw { set; get; }

        void Draw(SpriteBatch spriteBatch);
    }
}