using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TWrite = ldtk.LdtkJson;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentTypeWriter]
    public class LdtkWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(ldtk.Serialize.ToJson(value));
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "UmbrellaToolsKit.Ldtk.LdtkReader, UmbrellaToolsKit";
        }
    }
}
