using System;
using Microsoft.Xna.Framework.Content;

namespace UmbrellaToolsKit.Ldtk
{
    public class LdtkReader : ContentTypeReader<ldtk.LdtkJson>
    {
        protected override ldtk.LdtkJson Read(ContentReader input, ldtk.LdtkJson existingInstance)
        {
            string text = input.ReadString();
            return ldtk.LdtkJson.FromJson(text);
        }
    }
}