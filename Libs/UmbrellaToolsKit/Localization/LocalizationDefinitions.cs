using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Localization
{
    public class LocalizationDefinitions
    {
        public List<string> Tags { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Translation { get; set; }

        public LocalizationDefinitions()
        {
            this.Tags = new List<string>();
            this.Languages = new List<string>();
            this.Translation = new List<string>();
        }

        public LocalizationDefinitions(List<string> tags, List<string> languages, List<string> translations)
        {
            this.Tags = tags;
            this.Languages = languages;
            this.Translation = translations;
        }


        public string Get(string lang, string tag)
        {
            int tag_i = this.Tags.Select((item, index) => new
            {
                ItemName = item,
                Postion = index
            }).Where(t => t.ItemName == tag).First().Postion;

            int lang_i = this.Languages.Select((item, index) => new
            {
                ItemName = item,
                Position = index
            }).Where(l => l.ItemName == lang).First().Position;

            if (tag_i < this.Tags.Count && tag_i >= 0)
                return this.Translation[(lang_i * this.Tags.Count) + tag_i];

            return tag;
        }

    }
}
