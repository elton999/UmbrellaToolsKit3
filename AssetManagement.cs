using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;

namespace UmbrellaToolKit
{
    public class AssetManagement
    {
        public List<AssetObject> AssetsList = new List<AssetObject>();
        public List<AssetObject> LevelAssetsList = new List<AssetObject>();

        public Scene Scene { get; set; }

        public void Set<T>(string tag, string layer) where T : GameObject
        {
            AssetObject assetObject = new AssetObject { Name = tag, Layer = layer, GameObject = typeof(T) };
            this.AssetsList.Add(assetObject);
        }
        
        public GameObject GetObject(string name)
        {
            IEnumerable<AssetObject> assetObjects = this.AssetsList.Where( asset => asset.Name == name);

            if (assetObjects.Count() > 0)
            {
                AssetObject assetObject = assetObjects.First();
                GameObject gameObject = (GameObject)Activator.CreateInstance(assetObject.GameObject);
                gameObject.Content = this.Scene.Content;
                gameObject.Scene = this.Scene;
                gameObject.Start();
                return gameObject;
            }

            return new GameObject();
        }

        public string GetLayer(string name)
        {
            IEnumerable<AssetObject> assetObjects = this.AssetsList.Where(asset => asset.Name == name);
            if (assetObjects.Count() > 0)
            {
                return assetObjects.ToList()[0].Layer;
            }
            return "";
        }


        public void addEntityOnScene(string name, Vector2 position, Point size){ // ? values:Dynamic, ? nodes:Array<Vector2>, ? flipx:Bool):Void{
            GameObject gameObject = this.GetObject(name);
            string layer = this.GetLayer(name);

            gameObject.Position = position;
            gameObject.size = size;

            if (layer == "PLAYER")
                this.Scene.Players.Add(gameObject);
            else if (layer == "ENEMIES")
                this.Scene.Enemies.Add(gameObject);
            else if (layer == "FOREGROUND")
                this.Scene.Foreground.Add(gameObject);
            else if (layer == "MIDDLEGROUND")
                this.Scene.Middleground.Add(gameObject);
            else if (layer == "BACKGROUND")
                this.Scene.Backgrounds.Add(gameObject);

            gameObject.Start();
        }


        public void ClearAll()
        {
                this.LevelAssetsList.Clear();
        }
    }


    public class AssetObject
    {
        public string Layer;
        public string Name;
        public Vector2 Position;
        public Type GameObject;
    }
}
