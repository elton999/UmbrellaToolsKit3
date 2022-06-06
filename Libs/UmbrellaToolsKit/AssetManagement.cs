using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class AssetManagement
    {
        public static AssetManagement Instance;
        public List<AssetObject> AssetsList = new List<AssetObject>();
        public List<AssetObject> LevelAssetsList = new List<AssetObject>();

        public AssetManagement() => Instance = this;

        public void Set<T>(string tag, Layers layer) where T : GameObject
        {
            AssetObject assetObject = new AssetObject { Name = tag, Layer = layer, GameObject = typeof(T) };
            this.AssetsList.Add(assetObject);
        }

        public GameObject GetObject(string name)
        {
            IEnumerable<AssetObject> assetObjects = this.AssetsList.Where(asset => asset.Name == name);

            if (assetObjects.Count() > 0)
            {
                AssetObject assetObject = assetObjects.First();
                GameObject gameObject = (GameObject)Activator.CreateInstance(assetObject.GameObject);

                return gameObject;
            }

            return new GameObject();
        }

        public Layers GetLayer(string name)
        {
            IEnumerable<AssetObject> assetObjects = this.AssetsList.Where(asset => asset.Name == name);
            if (assetObjects.Count() > 0)
                return assetObjects.ToList()[0].Layer;
            throw new ArgumentOutOfRangeException();
        }

        public void addEntityOnScene(string name, Vector2 position, Point size, Dictionary<string, string> values, List<Vector2> nodes, Scene scene)
        { // ? values:Dynamic, ? nodes:Array<Vector2>, ? flipx:Bool):Void{
            GameObject gameObject = GetObject(name);
            var layer = GetLayer(name);

            gameObject.Position = position;
            gameObject.size = size;
            gameObject.Values = values;
            gameObject.Nodes = nodes;
            SetLayerToGameObject(scene, gameObject, layer);

            gameObject.Content = scene.Content;
            gameObject.Scene = scene;

            gameObject.Start();
        }

        public static void SetLayerToGameObject(Scene scene, GameObject gameObject, Layers layer)
        {
            switch (layer)
            {
                case Layers.PLAYER:
                    scene.Players.Add(gameObject);
                    break;
                case Layers.ENEMIES:
                    scene.Enemies.Add(gameObject);
                    break;
                case Layers.FOREGROUND:
                    scene.Foreground.Add(gameObject);
                    break;
                case Layers.MIDDLEGROUND:
                    scene.Middleground.Add(gameObject);
                    break;
                case Layers.BACKGROUND:
                    scene.Backgrounds.Add(gameObject);
                    break;
            }
        }

        public void ClearAll() => LevelAssetsList.Clear();

    }
}
