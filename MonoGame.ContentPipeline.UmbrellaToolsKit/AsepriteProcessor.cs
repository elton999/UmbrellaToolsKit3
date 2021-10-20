using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json.Linq;
using TInput = System.String;
using TOutput = Newtonsoft.Json.Linq.JObject;
//using TOutput = MonoGame.ContentPipeline.UmbrellaToolKit.Model.AsepriteJson;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentProcessor(DisplayName = "Aseprite Processor - UmbrellaToolKit")]
    public class AsepriteProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return JObject.Parse(input);
            //return JsonConvert.DeserializeObject<Model.AsepriteJson>(input);
        }
    }
}
