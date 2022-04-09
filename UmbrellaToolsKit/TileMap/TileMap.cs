using System;
using System.Linq;
using System.Collections.Generic;
using UmbrellaToolsKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.Sprite;

namespace UmbrellaToolsKit.TileMap
{
    public class TileMap
    {
        public static void Create(Scene scene, Ogmo.TileMap tileMap, Texture2D tilemapSprite)
        {
            SetSceneSizes(scene, tileMap);

            foreach (Ogmo.TileMapLayers layer in tileMap.layers)
            {
                if (layer.grid2D.Count() > 0)
                    SetGrid(scene, tileMap, layer);
                else if (layer.dataCoords2D.Count() > 0)
                    SetTiles(scene, tileMap, tilemapSprite, layer);
                else if (layer.entities.Count() > 0)
                    SetEntities(scene, layer);
            }
        }

        public static void Create(Scene scene, ldtk.LdtkJson tileMap, string levelName, Texture2D tilemapSprite)
        {
            ldtk.Level level = GetLevelByName(tileMap, levelName);
            SetSceneSizes(scene, level);

            for (var i = 0; i < level.LayerInstances.Length; i++)
            {
                var layer = level.LayerInstances[i];
                string layerType = layer.Type;
                if (layerType == "IntGrid")
                {
                    SetGrid(scene, layer);
                    SetTiles(scene, tilemapSprite, layer);
                }
                else if (layerType == "Entities")
                    SetEntities(scene, layer);
            }
        }

        private static void SetSceneSizes(Scene scene, Ogmo.TileMap tileMap)
        {
            scene.ScreemOffset = new Point(tileMap.offsetX, tileMap.offsetY);
            scene.LevelSize = new Vector2(tileMap.width, tileMap.height);
        }

        private static void SetSceneSizes(Scene scene, ldtk.Level level)
        {
            scene.ScreemOffset = new Point((int)level.WorldX, (int)level.WorldY);
            scene.LevelSize = new Vector2((int)level.PxWid, (int)level.PxHei);
        }

        private static ldtk.Level GetLevelByName(ldtk.LdtkJson tileMap, string levelName)
        {
            return (from levels in tileMap.Levels where levels.Identifier.Equals(levelName) select levels).First();
        }

        private static void SetEntities(Scene scene, ldtk.LayerInstance layer)
        {
            Console.WriteLine($"Loading Entities: {layer.Identifier} ");

            for (var i = 0; i < layer.EntityInstances.Length; i++)
            {
                Console.Write(".");
                // TODO: values and nodes
                var entity = layer.EntityInstances[i];
                AssetManagement.Instance.addEntityOnScene(
                        entity.Identifier,
                        new Vector2(entity.Px[0] + scene.ScreemOffset.X, entity.Px[1] + scene.ScreemOffset.Y),
                        new Point((int)entity.Width, (int)entity.Height),
                        null,
                        null,
                        scene
                    );
            }
        }

        private static void SetEntities(Scene scene, Ogmo.TileMapLayers layer)
        {
            Console.WriteLine($"Loading Entities: {layer.name} ");
            foreach (Ogmo.TileMapEntity entity in layer.entities)
            {
                System.Console.Write(".");
                if (AssetManagement.Instance != null)
                {
                    AssetManagement.Instance.addEntityOnScene(
                        entity.name,
                        new Vector2(entity.x + scene.ScreemOffset.X, entity.y + scene.ScreemOffset.Y),
                        new Point(entity.width, entity.height),
                        entity.values,
                        entity.nodes,
                        scene
                    );
                }
            }
        }

        private static void SetGrid(Scene scene, Ogmo.TileMap tileMap, Ogmo.TileMapLayers layer)
        {
            scene.Grid = new Grid();
            scene.Grid.GridCollides = layer.grid2D;
            scene.Grid.Scene = scene;
            scene.Grid.Origin = new Vector2(tileMap.offsetX, tileMap.offsetY);
        }

        private static void SetGrid(Scene scene, ldtk.LayerInstance layer)
        {
            scene.Grid = new Grid();
            scene.Grid.Origin = new Vector2(scene.ScreemOffset.X, scene.ScreemOffset.Y);
            scene.Grid.Scene = scene;

            for (var i = 0; i < layer.IntGridCsv.Length; i++)
            {
                int height = (int)layer.CHei;
                int width = (int)layer.CWid;
                int x = (int)(i % width);
                int y = (int)(i / width);
                if (x == 0)
                    scene.Grid.GridCollides.Add(new List<string>());
                scene.Grid.GridCollides[y].Add(layer.IntGridCsv[i].ToString());
            }
        }

        private static void SetTiles(Scene scene, Ogmo.TileMap tileMap, Texture2D tilemapSprite, Ogmo.TileMapLayers layer)
        {
            Layer layerTiles = CreateLayer(scene, tilemapSprite);
            layerTiles.Origin = new Vector2(tileMap.offsetX, tileMap.offsetY);
            layerTiles.tiles = layer.dataCoords2D;
        }

        private static void SetTiles(Scene scene, Texture2D tilemapSprite, ldtk.LayerInstance layer)
        {
            Layer layerTiles = CreateLayer(scene, tilemapSprite);
            layerTiles.Origin = new Vector2(scene.ScreemOffset.X, scene.ScreemOffset.Y);
            layerTiles.tiles = new List<List<List<int>>>();

            CreateTiles(layer, layerTiles);

            for (var i = 0; i < layer.AutoLayerTiles.Count(); i++)
            {
                var tile = layer.AutoLayerTiles[i];
                int x = (int)tile.Px[0] / 8;
                int y = (int)tile.Px[1] / 8;
                layerTiles.tiles[y][x][0] = (int)tile.Src[0] / 8;
                layerTiles.tiles[y][x].Add((int)(tile.Src[1] / 8));
            }
        }

        private static void CreateTiles(ldtk.LayerInstance layer, Layer layerTiles)
        {
            for (var x = 0; x < layer.CHei; x++)
            {
                layerTiles.tiles.Add(new List<List<int>>());
                for (var y = 0; y < layer.CWid; y++)
                {
                    layerTiles.tiles[x].Add(new List<int>());
                    layerTiles.tiles[x][y].Add(-1);
                }
            }
        }

        private static Layer CreateLayer(Scene scene, Texture2D tilemapSprite)
        {
            var layerTiles = new Sprite.Layer();
            layerTiles.Sprite = tilemapSprite;
            layerTiles.Scene = scene;

            scene.Backgrounds.Add(layerTiles);
            return layerTiles;
        }
    }
}
