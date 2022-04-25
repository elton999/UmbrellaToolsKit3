using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using UmbrellaToolsKit.Collision;

namespace UmbrellaToolsKit
{
    public class Scene : IDisposable
    {
        public Scene(GraphicsDevice screenGraphicsDevice, ContentManager content)
        {
            ScreenGraphicsDevice = screenGraphicsDevice;
            Content = content;
            addLayers();
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

        public void addLayers()
        {
            SortLayers = new List<List<GameObject>>();
            SortLayers.Add(Foreground);
            SortLayers.Add(Players);
            SortLayers.Add(Enemies);
            SortLayers.Add(Middleground);
            SortLayers.Add(Backgrounds);
        }
        #endregion

        #region Setting Scene
        //Sizes
        private int Width = 426;
        private int Height = 240;
        public GraphicsDevice ScreenGraphicsDevice;
        public ContentManager Content;

        private Color BackgroundColor = Color.CornflowerBlue;
        public ScreenController Screen { get; set; }

        //Camera
        public Point ScreenOffset;
        private Vector2 CamPositionz;
        public CameraController Camera;

        public bool LevelReady = false;

        public void SetSizes(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Point Sizes { get => new Point(Width, Height); }

        public Color SetBackgroundColor
        {
            set => BackgroundColor = value;
        }
        #endregion

        #region Load Level Tilemap
        public Ogmo.TileSet tileSet;
        public GameManagement GameManagement;

        public string MapLevelPath = "Maps/level_";
        public string MapLevelLdtkPath = "Maps/TileMap";
        public string TileMapPath = "Sprites/tilemap";

        public Vector2 LevelSize;

        public void SetLevel(int level)
        {
            System.Console.WriteLine($"Level: {MapLevelPath + level}");
            CreateCamera();

            Ogmo.TileMap tileMap = Content.Load<Ogmo.TileMap>(MapLevelPath + level);

            Texture2D _tilemapSprite = Content.Load<Texture2D>(TileMapPath);

            TileMap.TileMap.Create(this, tileMap, _tilemapSprite);
            CreateBackBuffer();

            LevelReady = true;
            System.Console.WriteLine("\nDone");
        }

        public void SetLevelLdtk(int level)
        {
            System.Console.WriteLine($"Level: {MapLevelLdtkPath}");
            Texture2D _tilemapSprite = Content.Load<Texture2D>(TileMapPath);

            var tileMap = Content.Load<ldtk.LdtkJson>(MapLevelLdtkPath);

            TileMap.TileMap.Create(this, tileMap, "Level_" + level, _tilemapSprite);
        }

        public void CreateCamera()
        {
            Camera = new CameraController();
            Camera.Scene = this;
        }

        public void CreateBackBuffer() => _BackBuffer = new RenderTarget2D(ScreenGraphicsDevice, Width, Height);
        #endregion

        #region Update
        public float timer = 0;
        public float updateDataTime = 1 / 30f;
        private void UpdateGameObjects(GameTime gameTime, List<List<GameObject>> layers)
        {
            //UI update
            for (int i = UI.Count - 1; i >= 0; i--)
                UI[i].Update(gameTime);

            for (int i = layers.Count - 1; i >= 0; i--)
                for (int e = layers[i].Count - 1; e >= 0; e--)
                    layers[i][e].Update(gameTime);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (timer >= updateDataTime)
            {
                for (int i = layers.Count - 1; i >= 0; i--)
                {
                    for (int e = layers[i].Count - 1; e >= 0; e--)
                    {
                        layers[i][e].UpdateData(gameTime);

                        if (Camera != null)
                            Camera.update(gameTime);
                    }

                }
                if (Camera != null)
                    Camera.CheckActorAndSolids();
                timer -= updateDataTime;
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
                // remove gameObjects
                RemoveGameObject(SortLayers);
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
            IEnumerable<GameObject> _UI_Objects_to_remove = from gameObject in UI where gameObject.RemoveFromScene == true select gameObject;

            IEnumerable<GameObject> _UI_Objects = from gameObject in UI where gameObject.RemoveFromScene == false select gameObject;
            UI = _UI_Objects.ToList<GameObject>();

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
        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Vector2 Viewport)
        {
            if (LevelReady)
            {
                DrawGameObjectsBeforeScene(spriteBatch, SortLayers);

                //UI Draw before scene
                for (int i = UI.Count - 1; i >= 0; i--)
                    UI[i].DrawBeforeScene(spriteBatch);


                RestartRenderTarget();
                if (BackgroundColor != Color.Transparent) graphicsDevice.Clear(BackgroundColor);

                DrawGameObjects(spriteBatch, SortLayers);

#if DEBUG_COLLISION
                if (this.Grid != null)
                    this.Grid.Draw(spriteBatch);
#endif
                //UI Draw
                for (int i = UI.Count - 1; i >= 0; i--)
                    UI[i].Draw(spriteBatch);
            }

            //Scale canvas settings
            float _xScale = Viewport.X / Width;
            float _yScale = Viewport.Y / Height;
            float _BackBuffer_scale = Viewport.X < Viewport.Y ? _xScale : _yScale;

            float _BackBuffer_Position_x = ((Viewport.X / 2) - (Width * _BackBuffer_scale / 2));
            float _BackBuffer_Position_y = ((Viewport.Y / 2) - (Height * _BackBuffer_scale / 2));

            ScreenGraphicsDevice.SetRenderTarget(null);
            ScreenGraphicsDevice.Clear(ClearColorScene);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(
                (Texture2D)_BackBuffer,
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
            this._BackBuffer.Dispose();

            foreach (List<GameObject> layer in SortLayers)
                foreach (GameObject gameObject in layer)
                    gameObject.Dispose();

            foreach (GameObject gameObject in UI)
                gameObject.Dispose();

            Grid.Dispose();

            GC.SuppressFinalize(this);
            GC.Collect();

            LevelReady = false;
        }
    }
}