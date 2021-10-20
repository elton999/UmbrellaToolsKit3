using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using TInput = System.String;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentImporter(".json", DisplayName = "Aseprite Importer - UmbrellaToolKit", DefaultProcessor = "AsepriteProcessor")]
    public class AsepriteImporter : ContentImporter<TInput>
    {
        public override TInput Import(string filename, ContentImporterContext context)
        {
            using (StreamReader stream = new StreamReader(filename))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
