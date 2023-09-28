using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class AssetManagement
    {
        public static AssetManagement Instance;
        public AssetManagement()
        {
            Instance = this;
        }
        public List<AssetObject> AssetsList = new List<AssetObject>();
        public List<AssetObject> LevelAssetsList = new List<AssetObject>();

        public void Set<T>(string tag, Layers layer) where T : GameObject
        {
            AssetObject assetObject = new AssetObject { Name = tag, Layer = layer, GameObject = typeof(T) };
            this.AssetsList.Add(assetObject);
        }

        public IEnumerable<AssetObject> GetObject(string name)
        {
            IEnumerable<AssetObject> assetObjects = this.AssetsList.Where(asset => asset.Name == name);

            return assetObjects;
        }

        public void addEntityOnScene(string name, Vector2 position, Point size, Dictionary<string, string> values, List<Vector2> nodes, Scene scene)
        { // ? values:Dynamic, ? nodes:Array<Vector2>, ? flipx:Bool):Void{
            var assets = this.GetObject(name);

            foreach (var asset in assets)
            {
                GameObject gameObject = (GameObject)Activator.CreateInstance(asset.GameObject);
                gameObject.Position = position;
                gameObject.size = size;
                gameObject.Values = values;
                gameObject.Nodes = nodes;

                scene.AddGameObject(gameObject, asset.Layer);
            }
        }


        public void ClearAll()
        {
            this.LevelAssetsList.Clear();
        }
    }
}
