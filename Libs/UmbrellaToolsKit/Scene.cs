using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using UmbrellaToolsKit.Collision;
using UmbrellaToolsKit.Utils;

namespace UmbrellaToolsKit
{
    public class Scene : IDisposable
    {
        public Scene(GraphicsDevice screenGraphicsDevice, ContentManager content)
        {
            ScreenGraphicsDevice = screenGraphicsDevice;
            Content = content;
            addLayers();
#if DEBUG
            AddGameObject(new CheatListener(), Layers.BACKGROUND);
#endif
        }

        #region Layers
        public List<List<GameObject>> SortLayers = new List<List<GameObject>>();

        // UI
        public List<GameObject> UI = new List<GameObject>();
        // Layers
        public List<GameObject> Foreground = new List<GameObject>();
        public List<GameObject> Players = new List<GameObject>();
        public List<GameObject> Enemies = new List<GameObject>();
        public List<GameObject> Middleground = new List<GameObject>();
        public List<GameObject> Backgrounds = new List<GameObject>();

        // Collision
        public List<Solid> AllSolids = new List<Solid>();
        public List<Actor> AllActors = new List<Actor>();
        public Grid Grid;
        public int CellSize = 8;
        public bool PixelArt = true;

        public Effect Effect;

        public void addLayers()
        {
            SortLayers = new List<List<GameObject>>
            {
                Foreground,
                Players,
                Enemies,
                Middleground,
                Backgrounds
            };
        }

        public void AddGameObject(GameObject gameObject, Layers layer = Layers.MIDDLEGROUND)
        {
            switch (layer)
            {
                case Layers.PLAYER:
                    Players.Add(gameObject);
                    break;
                case Layers.ENEMIES:
                    Enemies.Add(gameObject);
                    break;
                case Layers.UI:
                    UI.Add(gameObject);
                    break;
                case Layers.FOREGROUND:
                    Foreground.Add(gameObject);
                    break;
                case Layers.MIDDLEGROUND:
                    Middleground.Add(gameObject);
                    break;
                case Layers.BACKGROUND:
                    Backgrounds.Add(gameObject);
                    break;
            }

            gameObject.Scene = this;
            gameObject.Layer = layer;
            gameObject.Content = Content;
            gameObject.Start();
        }

        #endregion

        #region Setting Scene
        //Sizes
        private int Width = 256;
        private int Height = 144;
        public GraphicsDevice ScreenGraphicsDevice;
        public ContentManager Content;

        private Color backgroundColor = Color.CornflowerBlue;
        public ScreenController Screen { get; set; }

        //Camera
        public Point ScreenOffset;
        public CameraController Camera;

        public bool LevelReady = false;

        public void SetSizes(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Point Sizes { get => new Point(Width, Height); }

        public Color BackgroundColor
        {
            set => backgroundColor = value;
            get => backgroundColor;
        }
        #endregion

        #region Load Level Tilemap
        public Ogmo.TileSet tileSet;
        public GameManagement GameManagement;

        public string MapLevelPath = "Maps/Level_";
        public string MapLevelLdtkPath = "Maps/TileMap";
        public string TileMapPath = "Sprites/tilemap";

        public Vector2 LevelSize;

        public void SetLevel(int level) => SetLevel(MapLevelPath + level);

        public void SetLevel(string level)
        {
            EditorEngine.Log.Write($"Level: {MapLevelPath + level}");
            CreateCamera();

            Ogmo.TileMap tileMap = Content.Load<Ogmo.TileMap>(level);

            Texture2D _tilemapSprite = Content.Load<Texture2D>(TileMapPath);

            TileMap.TileMap.Create(this, tileMap, _tilemapSprite);
            CreateBackBuffer();

            LevelReady = true;
            EditorEngine.Log.Write("\nDone");
        }

        public void SetLevelLdtk(int level)
        {
            EditorEngine.Log.Write($"Level: {MapLevelLdtkPath}");
            EditorEngine.Log.Write($"tilemap sprite: {TileMapPath}");

            CreateCamera();
            CreateBackBuffer();

            Texture2D _tilemapSprite = Content.Load<Texture2D>(TileMapPath);
            var tileMap = Content.Load<ldtk.LdtkJson>(MapLevelLdtkPath);
            TileMap.TileMap.Create(this, tileMap, "Level_" + level, _tilemapSprite);

            LevelReady = true;
            EditorEngine.Log.Write("\nDone");
        }

        public void CreateCamera()
        {
            Camera = new CameraController();
            Camera.Scene = this;
        }

        public void CreateBackBuffer() => _BackBuffer = new RenderTarget2D(ScreenGraphicsDevice, Width, Height);
        #endregion

        #region Update
        public float deltaTimerData = 0;
        public float updateDataTime = 1.0f / 30.0f;
        private void UpdateGameObjects(GameTime gameTime, List<List<GameObject>> layers)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //UI update
            for (int i = UI.Count - 1; i >= 0; i--)
            {
                UI[i].Update(deltaTime);
                UI[i].CoroutineManagement.Update(gameTime);
            }

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                for (int e = layers[i].Count - 1; e >= 0; e--)
                {
                    if (layers[i][e].Components != null)
                    {
                        var component = layers[i][e].Components;
                        while (component != null)
                        {
                            component.Update(deltaTime);
                            component = component.Next;
                        }
                    }
                    try
                    {
                        layers[i][e].Update(deltaTime);
                        layers[i][e].CoroutineManagement.Update(gameTime);
                    }
                    catch { }
                }
            }

            deltaTimerData += deltaTime;
            if (deltaTimerData >= MathUtils.MillisecondsToSeconds(updateDataTime))
            {
                for (int i = layers.Count - 1; i >= 0; i--)
                {
                    for (int e = layers[i].Count - 1; e >= 0; e--)
                    {

                        if (Camera != null)
                            Camera.update(deltaTimerData);

                        if (layers[i][e].Components != null)
                        {
                            var component = layers[i][e].Components;
                            while (component != null)
                            {
                                component.UpdateData(deltaTimerData);
                                component = component.Next;
                            }
                        }
                        try
                        {
                            layers[i][e].UpdateData(deltaTimerData);
                        }
                        catch { }
                    }
                }
                if (Camera != null)
                    Camera.CheckActorAndSolids();
                deltaTimerData = 0.0f;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (LevelReady)
            {
                // Update gameObjects
                UpdateGameObjects(gameTime, SortLayers);
                // check if gameobjects is visible
                IsVisibleGameObject(SortLayers);

            }
        }

        public void IsVisibleGameObject(List<List<GameObject>> layers)
        {
            for (int i = layers.Count - 1; i >= 0; i--)
                for (int e = layers[i].Count - 1; e >= 0; e--)
                    if (CheckIsVisible(layers[i][e]))
                        layers[i][e].OnVisible();
                    else
                        layers[i][e].OnInvisible();
        }

        private bool CheckIsVisible(GameObject gameObject)
        {
            Vector2 _gameObjectPosition = gameObject.Position;

            if (Screen != null)
            {
                bool overlay_x = false;
                bool overlay_y = false;
                Vector2 CameraPosition = -Screen.CameraManagement.Position;

                if (CameraPosition.X - (ScreenOffset.X / 2f) < _gameObjectPosition.X && CameraPosition.X + (ScreenOffset.X / 2f) > _gameObjectPosition.X)
                    overlay_x = true;

                if (CameraPosition.Y - (ScreenOffset.Y / 2f) < _gameObjectPosition.Y && CameraPosition.Y + (ScreenOffset.Y / 2f) > _gameObjectPosition.Y)
                    overlay_y = true;

                if (overlay_x && overlay_y)
                    return true;
            }

            return false;
        }

        private void RemoveGameObject(List<List<GameObject>> layers)
        {
            // UI
            for (int i = UI.Count - 1; i >= 0; i--)
            {
                if (UI[i].RemoveFromScene)
                    UI.RemoveAt(i);
            }

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                for (int e = layers[i].Count - 1; e >= 0; e--)
                {
                    if (layers[i][e].RemoveFromScene)
                    {
                        layers[i][e].Destroy();
                        layers[i].RemoveAt(e);
                    }
                }
            }
        }
        #endregion

        #region Draw
        private RenderTarget2D _BackBuffer;
        public Texture2D SceneRendered { get => (Texture2D)_BackBuffer; }
        public void RestartRenderTarget()
        {
            ScreenGraphicsDevice.SetRenderTarget(this._BackBuffer);
        }

        private void DrawGameObjects(SpriteBatch spriteBatch, List<List<GameObject>> layers)
        {
            for (int i = layers.Count - 1; i >= 0; i--)
                for (int e = layers[i].Count - 1; e >= 0; e--)
                    if (!layers[i][e].RemoveFromScene)
                        layers[i][e].Draw(spriteBatch);
        }

        private void DrawGameObjectsBeforeScene(SpriteBatch spriteBatch, List<List<GameObject>> layers)
        {
            for (int i = layers.Count - 1; i >= 0; i--)
                for (int e = layers[i].Count - 1; e >= 0; e--)
                    if (!layers[i][e].RemoveFromScene)
                        layers[i][e].DrawBeforeScene(spriteBatch);
        }

        public Color ClearColorScene = Color.Black;

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Vector2 Viewport)
        {
            if (LevelReady)
            {
                DrawGameObjectsBeforeScene(spriteBatch, SortLayers);

                //UI Draw before scene
                for (int i = 0; i < UI.Count; i++)
                    if (!UI[i].RemoveFromScene)
                        UI[i].DrawBeforeScene(spriteBatch);


                RestartRenderTarget();
                if (backgroundColor != Color.Transparent) graphicsDevice.Clear(backgroundColor);

                DrawGameObjects(spriteBatch, SortLayers);

#if DEBUG_COLLISION
                if (this.Grid != null)
                    this.Grid.Draw(spriteBatch);
#endif
                //UI Draw
                for (int i = 0; i < UI.Count; i++)
                    if (!UI[i].RemoveFromScene)
                        UI[i].Draw(spriteBatch);

                // remove gameObjects
                RemoveGameObject(SortLayers);
            }

            //Scale canvas settings
            float _xScale = Viewport.X / Width;
            float _yScale = Viewport.Y / Height;
            float _BackBuffer_scale = Viewport.X < Viewport.Y ? _xScale : _yScale;

            float _BackBuffer_Position_x = ((Viewport.X / 2) - (Width * _BackBuffer_scale / 2));
            float _BackBuffer_Position_y = ((Viewport.Y / 2) - (Height * _BackBuffer_scale / 2));

            ScreenGraphicsDevice.SetRenderTarget(null);
            ScreenGraphicsDevice.Clear(ClearColorScene);
            DrawBuffer(spriteBatch, _BackBuffer_scale, _BackBuffer_Position_x, _BackBuffer_Position_y);
        }

        public void DrawBuffer(SpriteBatch spriteBatch, float _BackBuffer_scale, float _BackBuffer_Position_x, float _BackBuffer_Position_y)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, Effect != null ? Effect : null, null);
            spriteBatch.Draw(
                _BackBuffer,
                new Vector2(_BackBuffer_Position_x, _BackBuffer_Position_y),
                null,
                Color.White,
                0,
                Vector2.Zero,
                _BackBuffer_scale,
                SpriteEffects.None,
                0
            );
            spriteBatch.End();
        }
        #endregion

        public void Dispose()
        {
            LevelReady = false;
            if (_BackBuffer != null)
                _BackBuffer.Dispose();
            _BackBuffer = null;

            foreach (List<GameObject> layer in SortLayers)
            {
                foreach (GameObject gameObject in layer)
                    gameObject.Dispose();
                layer.Clear();
            }

            foreach (GameObject gameObject in UI)
                gameObject.Dispose();
            UI.Clear();

            AllSolids.Clear();
            AllActors.Clear();

            if (Grid != null) Grid.Dispose();
            Grid = null;

            GC.SuppressFinalize(this);
            GC.Collect();

            LevelReady = false;
        }
    }
}