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
            int framesCount = ((JArray)value["frames"]).Count;
            output.Write(framesCount);
            for (int i = 0; i < framesCount; i++)
            {
                output.Write((int)value["frames"][i]["frame"]["x"]);
                output.Write((int)value["frames"][i]["frame"]["y"]);
                output.Write((int)value["frames"][i]["frame"]["w"]);
                output.Write((int)value["frames"][i]["frame"]["h"]);
                output.Write((int)value["frames"][i]["duration"]);
            }

            // slices
            int slicesCount = ((JArray)value["meta"]["slices"]).Count;
            output.Write(slicesCount);
            for (int i = 0; i < slicesCount; i++)
            {
                output.Write((string)value["meta"]["slices"][i]["name"]);
                output.Write(((JArray)value["meta"]["slices"][i]["keys"]).Count);
                for (int j = 0; j < ((JArray)value["meta"]["slices"][i]["keys"]).Count; j++)
                {
                    int frame = (int)value["meta"]["slices"][i]["keys"][j]["frame"];
                    output.Write(frame);
                    output.Write((int)value["meta"]["slices"][i]["keys"][j]["bounds"]["x"] + (int)value["frames"][frame]["frame"]["x"]);
                    output.Write((int)value["meta"]["slices"][i]["keys"][j]["bounds"]["y"] + (int)value["frames"][frame]["frame"]["y"]);
                    output.Write((int)value["meta"]["slices"][i]["keys"][j]["bounds"]["w"]);
                    output.Write((int)value["meta"]["slices"][i]["keys"][j]["bounds"]["h"]);

                    bool hasPivot = (value["meta"]["slices"][i]["keys"][j]["pivot"] != null);
                    output.Write(hasPivot);
                    if(hasPivot)
                    {
                        output.Write((int)value["meta"]["slices"][i]["keys"][j]["pivot"]["x"]);
                        output.Write((int)value["meta"]["slices"][i]["keys"][j]["pivot"]["y"]);
                    }
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "UmbrellaToolsKit.Sprite.AsepriteReader, UmbrellaToolsKit";
        }
    }
}
