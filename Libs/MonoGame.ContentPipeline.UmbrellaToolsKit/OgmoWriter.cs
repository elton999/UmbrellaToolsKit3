using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Newtonsoft.Json.Linq;
using System;

//using TWrite = MonoGame.ContentPipeline.UmbrellaToolKit.Model.AsepriteJson;
using TWrite = OgmoModel;


namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentTypeWriter]
    public class OgmoWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            // header
            output.Write((string)value.name);
            output.Write((string)value.ogmoVersion);
            output.Write((string)value.backgroundColor);
            output.Write((int)value.layerGridDefaultSize.x);
            output.Write((int)value.layerGridDefaultSize.y);

            // layers
            output.Write((int)value.layers.Count);
            for(var i = 0; i < value.layers.Count; i++)
            {
                output.Write((string)value.layers[i].exportID);
                output.Write((string)value.layers[i].definition);
                output.Write((string)value.layers[i].name);
                output.Write((int)value.layers[i].gridSize.x);
                output.Write((int)value.layers[i].gridSize.y);
                output.Write((int)value.layers[i].exportMode);
                output.Write((int)value.layers[i].arrayMode);
                
            }
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "UmbrellaToolsKit.Ogmo.TileSetReader, UmbrellaToolsKit";
        }
    }

    
}
