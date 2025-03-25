using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace UmbrellaToolsKit.Sprite
{
    public class AsepriteReader : ContentTypeReader<AsepriteDefinitions>
    {
        protected override AsepriteDefinitions Read(ContentReader input, AsepriteDefinitions existingInstance)
        {
            AsepriteDefinitions AsepriteDefinitions = new AsepriteDefinitions();
            int frameTagsCount = input.ReadInt32();
            for (int i = 0; i < frameTagsCount; i++)
            {
                string name = input.ReadString();
                string direction = input.ReadString();
                int from = input.ReadInt32();
                int to = input.ReadInt32();

                AsepriteDefinitions.TagAdd(name, direction, from, to);
            }

            int framesCount = input.ReadInt32();
            for (int i = 0; i < framesCount; i++)
            {
                int x = input.ReadInt32();
                int y = input.ReadInt32();
                int w = input.ReadInt32();
                int h = input.ReadInt32();
                int duration = input.ReadInt32();

                AsepriteDefinitions.BodyAdd(new Rectangle(new Point(x, y), new Point(w, h)));
                AsepriteDefinitions.Duration.Add(duration);
            }

            int slicesCount = input.ReadInt32();
            for(int i = 0; i < slicesCount; i++)
            {
                string name = input.ReadString();
                AsepriteDefinitions.Slices.Add(name, (new Rectangle(0,0,0,0), new Vector2(0,0)));
                
                int keysCount = input.ReadInt32();
                for(int j = 0; j < keysCount; j++)
                {
                    int frame = input.ReadInt32();
                    int x = input.ReadInt32();
                    int y = input.ReadInt32();
                    int w = input.ReadInt32();
                    int h = input.ReadInt32();

                    Vector2 origin = new Vector2(0,0);
                    bool hasPivot = input.ReadBoolean();
                    if(hasPivot)
                    {
                        int pivotX = input.ReadInt32();
                        int pivotY = input.ReadInt32();
                        origin = new Vector2(pivotX, pivotY);
                    }

                    Rectangle body = new Rectangle(x, y, w, h);
                    (Rectangle, Vector2) data = (body, origin);
                    AsepriteDefinitions.Slices[name] = data;
                }
            }

            return AsepriteDefinitions;
        }
    }
}
