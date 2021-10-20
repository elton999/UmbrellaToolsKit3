using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using TInput = System.String;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentImporter(".json", DisplayName = "Ogmo TileMap Importer - UmbrellaToolKit", DefaultProcessor = "OgmoTileMapProcessor")]
    public class OgmoTileMapImporter : ContentImporter<TInput>
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
