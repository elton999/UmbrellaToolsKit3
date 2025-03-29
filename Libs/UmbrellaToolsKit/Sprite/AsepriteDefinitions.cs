using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Sprite
{
    public class AsepriteDefinitions
    {
        public List<Rectangle> Bodys { get; set; }
        public List<int> Duration { get; set; }
        public List<AsepriteTags> Tags { get; set; }
        public Dictionary<string, (Rectangle, Vector2)> Slices { get; set; }

        public Rectangle Rectangle;

        public AsepriteDefinitions()
        {
            Bodys = new List<Rectangle>();
            Duration = new List<int>();
            Tags = new List<AsepriteTags>();
            Slices = new Dictionary<string, (Rectangle, Vector2)>();
        }

        public void BodyAdd(Rectangle body) => Bodys.Add(body);

        public void TagAdd(string name, string direction, int from, int to)
        {
            AsepriteTags AsepriteTags = new AsepriteTags
            {
                Name = name,
                direction = direction,
                from = from,
                to = to
            };
            Tags.Add(AsepriteTags);
        }
    }

    public class AsepriteTags
    {
        public string Name { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        public string direction { get; set; }
    }
}
