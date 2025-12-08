using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.EditorEngine;
using UmbrellaToolsKit.EditorEngine.Windows.GameSettings;

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
            UpdateSprite();
        }

        public void SetSprite(string path) => _sprite = new Sprite(GameObject.Content, path);

        public void SetAtlas(string spriteName)
        {
            var atlas = GameSettingsProperty.GetProperty<AtlasGameSettings>(@"Content/AtlasGameSettings");
            if (atlas.TryGetSpriteByName(spriteName, out var sprite))
            {
                Log.Write($"[SpriteComponent] creating sprite: path : {sprite.Path} rectangle: {sprite.GetRectangle()}" + spriteName);
                var spriteData = new Sprite(GameObject.Content, sprite.Path, sprite.GetRectangle());
                SetSprite(spriteData);
                return;
            }

            Log.Write("[SpriteComponent] Sprite not found in atlas: " + spriteName);
        }

        public void SetSprite(Sprite sprite)
        {
            _sprite = sprite;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            if (_sprite == null) return;

            GameObject.Sprite = _sprite.Texture;
            GameObject.Origin = _origin;
            GameObject.Transparent = _transparent;
            GameObject.SpriteEffect = _spriteEffect;
            GameObject.Body = _sprite.Body;
        }
    }
}
