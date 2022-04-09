using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Newtonsoft.Json.Linq;
using System;

//using TWrite = MonoGame.ContentPipeline.UmbrellaToolKit.Model.AsepriteJson;
using TWrite = Newtonsoft.Json.Linq.JObject;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentTypeWriter]
    public class AsepriteWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            // tags
            output.Write(((JArray)value["meta"]["frameTags"]).Count);
            for (int i = 0; i < ((JArray)value["meta"]["frameTags"]).Count; i++)
            {
                output.Write((string)value["meta"]["frameTags"][i]["name"]);
                output.Write((string)value["meta"]["frameTags"][i]["direction"]);
                output.Write((int)value["meta"]["frameTags"][i]["from"]);
                output.Write((int)value["meta"]["frameTags"][i]["to"]);
            }

            // frames
            output.Write(((JArray)value["frames"]).Count);
            for (int i = 0; i < ((JArray)value["frames"]).Count; i++)
            {
                output.Write((int)value["frames"][i]["frame"]["x"]);
                output.Write((int)value["frames"][i]["frame"]["y"]);
                output.Write((int)value["frames"][i]["frame"]["w"]);
                output.Write((int)value["frames"][i]["frame"]["h"]);
                output.Write((int)value["frames"][i]["duration"]);
            }    
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "UmbrellaToolsKit.Sprite.AsepriteReader, UmbrellaToolsKit";
        }
    }
}
