using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Components.Sprite
{
    public class SpriteComponent : Component
    {
        private Sprite _sprite;
        private Vector2 _origin;
        private float _transparent = 1.0f;
        private SpriteEffects _spriteEffect = SpriteEffects.None;

        public override void AfterUpdate(float deltaTime)
        {
            GameObject.Sprite = _sprite.Texture;
            GameObject.Origin = _origin;
            GameObject.Transparent = _transparent;
            GameObject.SpriteEffect = _spriteEffect;
        }

        public void SetSprite(string path) => _sprite = new Sprite(GameObject.Content, path);

        public void SetSprite(Sprite sprite) => _sprite = sprite;
    }
}
