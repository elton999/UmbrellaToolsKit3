using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Data.SqlTypes;

namespace UmbrellaToolsKit.Components.Sprite
{
    public class Sprite : INullable
    {
        private Rectangle _body;
        private string _path;
        private Texture2D _texture;
        private ContentManager _content;

        public  bool IsNull => _texture == null;

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
        

        public Sprite(ContentManager content, string path, Rectangle body)
        {
            _content = content;
            _path = path;
            _body = body;
            LoadSprite();
        }

        public Sprite(ContentManager content, string path)
        {
            _content = content;
            _path = path;
            LoadSprite();
        }

        private void LoadSprite()
        {
            _content.Load<Texture2D>(_path);
        }
    }
}
