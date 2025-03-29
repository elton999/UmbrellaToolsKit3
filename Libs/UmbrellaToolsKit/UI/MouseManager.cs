using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UmbrellaToolsKit.UI
{
    public class MouseManager : GameObject
    {
        public bool Show = false;

        public override void Update(float deltaTime) => Position = Mouse.GetState().Position.ToVector2();

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Show) return;
            base.Draw(spriteBatch);
        }

    }
}
