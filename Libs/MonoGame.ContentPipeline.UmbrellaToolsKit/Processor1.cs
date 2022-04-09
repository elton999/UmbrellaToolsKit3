using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.String;
using TOutput = System.String;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentProcessor(DisplayName = "Processor1")]
    class Processor1 : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return default(TOutput);
        }
    }
}
