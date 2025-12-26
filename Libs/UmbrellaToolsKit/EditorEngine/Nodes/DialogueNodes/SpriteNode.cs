using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    [NodeImplementation("DialogueNodes")]
    public class SpriteNode : NodeWithOptions
    {
        private Components.Sprite.Sprite _sprite = new Components.Sprite.Sprite(string.Empty, string.Empty, new Rectangle());
        private const string SPRITE_KEY = "sprite-name";

        protected override string _className => typeof(SpriteNode).Namespace + "." + typeof(SpriteNode).Name;

        public SpriteNode(Load storage, int id, string name, Vector2 position) : base(storage, id, "Sprite Node", position)
        { }

#if !RELEASE
        public override void DrawInspector()
        {
            Fields.Field.DrawSprite("Sprite", _sprite, out var newSprite);
            if (newSprite != null)
            {
                _sprite = newSprite;
            }
            base.DrawInspector();
        }

        public override void Load()
        {
            base.Load();
            var spriteName = _storage.getItemsString($"{SPRITE_KEY}-{Id}");
            if (spriteName != null && spriteName[0] != string.Empty)
            {
                _sprite = new Components.Sprite.Sprite(spriteName[0], string.Empty, new Rectangle());
            }
        }

        public override void OnSave()
        {
            base.OnSave();
            _storage.SetString($"{SPRITE_KEY}-{Id}", _sprite.Name);
        }

        public override void OnDelete()
        {
            base.OnDelete();
            _storage.DeleteNode($"{SPRITE_KEY}-{Id}");
        }
#endif
    }
}