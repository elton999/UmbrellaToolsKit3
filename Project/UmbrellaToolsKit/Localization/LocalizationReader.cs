using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace UmbrellaToolsKit.Localization
{
    public class LocalizationReader : ContentTypeReader<LocalizationDefinitions>
    {
        protected override LocalizationDefinitions Read(ContentReader input, LocalizationDefinitions existingInstance)
        {

            int languagesCount = input.ReadInt32();
            List<string> languages = new List<string>();
            for (int i = 0; i < languagesCount; i++)
            {
                languages.Add(input.ReadString());
            }

            int tagsCount = input.ReadInt32();
            List<string> tags = new List<string>();
            for (int i = 0; i < tagsCount; i++)
            {
                tags.Add(input.ReadString());
            }

            int translationsCount = input.ReadInt32();
            List<string> translation = new List<string>();
            for (int i = 0; i < translationsCount; i++)
            {
                translation.Add(input.ReadString());
            }

            LocalizationDefinitions Location = new LocalizationDefinitions(tags, languages, translation);
            return Location;
        }
    }
}
