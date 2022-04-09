using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using TInput = System.String;
using TOutput = OgmoModel;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentProcessor(DisplayName = "Ogmo Processor - UmbrellaToolKit")]
    public class OgmoProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return JsonConvert.DeserializeObject<TOutput>(input);
        }
    }
}
