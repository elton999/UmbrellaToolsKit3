using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using TInput = System.String;
using TOutput = OgmoLevel;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentProcessor(DisplayName = "Ogmo TileMap Processor  - UmbrellaToolKit")]
    public class OgmoTileMapProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return JsonConvert.DeserializeObject<TOutput>(input);
        }
    }
}
