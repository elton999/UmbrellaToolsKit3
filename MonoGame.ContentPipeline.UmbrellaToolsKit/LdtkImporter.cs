using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = System.String;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentImporter(".ldtk", DisplayName = "Ldtk Importer - UmbrellaToolKit", DefaultProcessor = "LdtkProcessor")]
    public class LdtkImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            using (StreamReader stream = new StreamReader(filename))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
