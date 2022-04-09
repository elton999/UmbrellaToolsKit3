using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.String;
using TOutput = ldtk.LdtkJson;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentProcessor(DisplayName = "Ldtk Processor - UmbrellaToolKit")]
    class LdtkProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return ldtk.LdtkJson.FromJson(input);
        }
    }
}
