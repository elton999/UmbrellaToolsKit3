using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using UmbrellaToolsKit.Collision;

namespace UmbrellaToolsKit
{
    public class Scene
    {
        public Scene(GraphicsDevice ScreemGraphicsDevice, ContentManager Content)
        {
            this.ScreemGraphicsDevice = ScreemGraphicsDevice;
            this.Content = Content;
            this.addLayers();
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
            this.SortLayers = new List<List<GameObject>>();
            this.SortLayers.Add(Foreground);
            this.SortLayers.Add(Players);
            this.SortLayers.Add(Enemies);
            this.SortLayers.Add(Middleground);
            this.SortLayers.Add(Backgrounds);
        }
        #endregion

        #region Setting Scene
        //Sizes
        private int Width = 426;
        private int Height = 240;
        public GraphicsDevice ScreemGraphicsDevice;
        public ContentManager Content;

        private Color BackgroundColor = Color.CornflowerBlue;
        public ScreemController Screem { get; set; }

        //Camera
        public Point ScreemOffset;
        private Vector2 CamPositionz;
        public CameraManagement Camera;

        public bool LevelReady = false;

        public void SetSizes(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Point Sizes { get => new Point(this.Width, this.Height); }

        public Color SetBackgroundColor
        {
            set => this.BackgroundColor = value;
        }
        #endregion

        #region Load Level Tilemap
        public Ogmo.TileMap tileMap;
        public Ogmo.TileSet tileSet;
        public GameManagement GameManagement;

        public string MapLevelPath = "Maps/level_";
        public string TileMapPath = "Sprites/tilemap";

        public Vector2 LevelSize;

        public void SetLevel(int level)
        {
            System.Console.WriteLine($"Level: {this.MapLevelPath + level}");
            this.CreateCamera();

            this.tileMap = Content.Load<Ogmo.TileMap>(this.MapLevelPath + level);

            Texture2D _tilemapSprite = Content.Load<Texture2D>(this.TileMapPath);

            this.tileMap.Create(this, this.tileMap, _tilemapSprite);
            this.CreateBackBuffer();

            this.LevelReady = true;
            System.Console.WriteLine("\nDone");
        }

        public void CreateCamera()
        {
            this.Camera = new CameraManagement();
            this.Camera.Scene = this;
        }

        public void CreateBackBuffer()
        {
            this._BackBuffer = new RenderTarget2D(ScreemGraphicsDevice, this.Width, this.Height);
        }
        #endregion

        #region Update
        public float timer = 0;
        public float updateDataTime = 1 / 30f;
        private void UpdateGameObjects(GameTime gameTime, List<List<GameObject>> layers)
        {
            //UI update
            for (int i = this.UI.Count - 1; i >= 0; i--)
                this.UI[i].Update(gameTime);

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

                        if (this.Camera != null)
                            this.Camera.update(gameTime);
                    }

                }
                if (this.Camera != null)
                    this.Camera.CheckActorAndSolids();
                timer -= updateDataTime;
            }

        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.LevelReady)
            {
                // Update gameObjects
                this.UpdateGameObjects(gameTime, SortLayers);
                // check if gameobjects is visible
                this.IsVisibleGameObject(SortLayers);
                // remove gameObjects
                this.RemoveGameObject(SortLayers);

            }
        }

        public void IsVisibleGameObject(List<List<GameObject>> layers)
        {
            for (int i = layers.Count - 1; i >= 0; i--)
                for (int e = layers[i].Count - 1; e >= 0; e--)
                    if (this.CheckIsVisible(layers[i][e]))
                        layers[i][e].OnVisible();
                    else
                        layers[i][e].OnInvisible();
        }

        private bool CheckIsVisible(GameObject gameObject)
        {
            Vector2 _gameObjectPosition = gameObject.Position;

            if (Screem != null)
            {
                bool overlay_x = false;
                bool overlay_y = false;
                Vector2 CameraPosition = -this.Screem.CameraManagement.Position;

                if (CameraPosition.X - (ScreemOffset.X / 2f) < _gameObjectPosition.X && CameraPosition.X + (ScreemOffset.X / 2f) > _gameObjectPosition.X)
                    overlay_x = true;

                if (CameraPosition.Y - (ScreemOffset.Y / 2f) < _gameObjectPosition.Y && CameraPosition.Y + (ScreemOffset.Y / 2f) > _gameObjectPosition.Y)
                    overlay_y = true;

                if (overlay_x && overlay_y)
                    return true;
            }

            return false;
        }

        private void RemoveGameObject(List<List<GameObject>> layers)
        {
            // UI
            IEnumerable<GameObject> _UI_Objects_to_remove = from gameObject in this.UI where gameObject.RemoveFromScene == true select gameObject;

            IEnumerable<GameObject> _UI_Objects = from gameObject in this.UI where gameObject.RemoveFromScene == false select gameObject;
            this.UI = _UI_Objects.ToList<GameObject>();

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
            ScreemGraphicsDevice.SetRenderTarget(this._BackBuffer);
        }

        private void DrawGameObjects(SpriteBatch spriteBatch, List<List<GameObject>> layers)
        {
            for (int i = layers.Count - 1; i >= 0; i--)
            {
                for (int e = layers[i].Count - 1; e >= 0; e--)
                {
                    if (!layers[i][e].RemoveFromScene) layers[i][e].Draw(spriteBatch);
                }
            }
        }

        private void DrawGameObjectsBeforeScene(SpriteBatch spriteBatch, List<List<GameObject>> layers)
        {
            for (int i = layers.Count - 1; i >= 0; i--)
            {
                for (int e = layers[i].Count - 1; e >= 0; e--)
                {
                    if (!layers[i][e].RemoveFromScene) layers[i][e].DrawBeforeScene(spriteBatch);
                }
            }
        }

        public Color ClearColorScene = Color.Black;
        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Vector2 Viewport)
        {
            if (this.LevelReady)
            {
                this.DrawGameObjectsBeforeScene(spriteBatch, SortLayers);

                //UI Draw before scene
                for (int i = this.UI.Count - 1; i >= 0; i--)
                    this.UI[i].DrawBeforeScene(spriteBatch);


                RestartRenderTarget();
                if (this.BackgroundColor != Color.Transparent) graphicsDevice.Clear(this.BackgroundColor);

                this.DrawGameObjects(spriteBatch, SortLayers);

#if DEBUG_COLLISION
                if (this.Grid != null)
                    this.Grid.Draw(spriteBatch);
#endif
                //UI Draw
                for (int i = this.UI.Count - 1; i >= 0; i--)
                    this.UI[i].Draw(spriteBatch);
            }

            

            //Scale canvas settings
            float _xScale = Viewport.X / this.Width;
            float _yScale = Viewport.Y / this.Height;
            float _BackBuffer_scale = Viewport.X < Viewport.Y ? _xScale : _yScale;

            float _BackBuffer_Position_x = ((Viewport.X / 2) - (this.Width * _BackBuffer_scale / 2));
            float _BackBuffer_Position_y = ((Viewport.Y / 2) - (this.Height * _BackBuffer_scale / 2));

            ScreemGraphicsDevice.SetRenderTarget(null);
            ScreemGraphicsDevice.Clear(this.ClearColorScene);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(
                (Texture2D)this._BackBuffer,
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
    }
}