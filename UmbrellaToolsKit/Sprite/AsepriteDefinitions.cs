using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Sprite
{
    public class AsepriteDefinitions
    {
        public List<Rectangle> Bodys { get; set; }
        public List<int> Duration { get; set; }
        public List<AsepriteTags> Tags { get; set; }

        public Rectangle Rectangle;

        public AsepriteDefinitions()
        {
            this.Bodys = new List<Rectangle>();
            this.Duration = new List<int>();
            this.Tags = new List<AsepriteTags>();
        }

        public void BodyAdd(Rectangle body)
        {
            this.Bodys.Add(body);
        }

        public void TagAdd(string name, string direction, int from, int to)
        {
            AsepriteTags AsepriteTags = new AsepriteTags();
            AsepriteTags.Name = name;
            AsepriteTags.direction = direction;
            AsepriteTags.from = from;
            AsepriteTags.to = to;
            this.Tags.Add(AsepriteTags);
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
