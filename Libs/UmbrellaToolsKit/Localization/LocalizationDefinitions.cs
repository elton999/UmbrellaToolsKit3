using System.Collections.Generic;
using System.Linq;

namespace UmbrellaToolsKit.Localization
{
    public class LocalizationDefinitions
    {
        public List<string> Tags { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Translation { get; set; }

        public LocalizationDefinitions()
        {
            Tags = new List<string>();
            Languages = new List<string>();
            Translation = new List<string>();
        }

        public LocalizationDefinitions(List<string> tags, List<string> languages, List<string> translations)
        {
            Tags = tags;
            Languages = languages;
            Translation = translations;
        }


        public string Get(string lang, string tag)
        {
            int tag_i = Tags.Select((item, index) => new
            {
                ItemName = item,
                Position = index
            }).Where(t => t.ItemName == tag).First().Position;

            int lang_i = Languages.Select((item, index) => new
            {
                ItemName = item,
                Position = index
            }).Where(l => l.ItemName == lang).First().Position;

            if (tag_i < Tags.Count && tag_i >= 0)
                return Translation[(lang_i * Tags.Count) + tag_i];

            return tag;
        }

    }
}
