using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Data.SqlTypes;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit.Components.Sprite
{
    [System.Serializable]
    public class Sprite : ISprite, INullable
    {
        private string _name;
        private Rectangle _body;
        private string _path;
        private Texture2D _texture;
        private ContentManager _content;

        public bool IsNull => _texture == null;

        public Rectangle Body
        {
            get
            {
                if (_body == null || _body.IsEmpty)
                    _body = new Rectangle(0, 0, Texture.Width, Texture.Height);
                return _body;
            }
            set { _body = value; }
        }

        public Texture2D Texture { get { return _texture; } set { _texture = value; } }

        public string Name { get => _name; set => _name = value; }
        public Vector2 Position { get => _body.Location.ToVector2(); set => _body.Location = value.ToPoint(); }
        public Vector2 Size { get => _body.Size.ToVector2(); set => _body.Size = value.ToPoint(); }
        public string Path { get => _path; set => _path = value; }

        public Sprite(ContentManager content, string path, Rectangle body)
        {
            _content = content;
            _path = path;
            _body = body;
            LoadSprite();
        }

        public Sprite(string name, string path, Rectangle body)
        {
            _name = name;
            _path = path;
            _body = body;
        }

        public Sprite(ContentManager content, string path)
        {
            _content = content;
            _path = path;
            LoadSprite();
        }

        public void SetContentManager(ContentManager content)
        {
            _content = content;
            LoadSprite();
        }

        public void SetTexture(Texture2D texture) => _texture = texture;

        public Texture2D GetTexture() => _texture;

        public Rectangle GetRectangle() => Body;

        private void LoadSprite() => _texture = _content.Load<Texture2D>(_path);
    }
}
