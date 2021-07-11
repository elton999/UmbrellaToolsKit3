# UmbrellaToolsKit
UmbrellaToolsKit is a framework made with Monogame for fast development.

## Get Started

### Basic Settings
Game1.cs file
``` csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolKit;

namespace ProjectMoon
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Scene MainScene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Setting the MainScene
            this.MainScene = new Scene(GraphicsDevice, Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            this.MainScene.Update(gametime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.MainScene.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}

```
### Loading Levels from OGMO Edtitor
``` csharp
public AssetManagement AssetManagement
protected override void LoadContent()
{
    _spriteBatch = new SpriteBatch(GraphicsDevice);
    // Loading costum Entities
    AssetManagement = new AssetManagement();
    AssetManagement.Set<Entities.Player.Player>("player", "PLAYER");
    AssetManagement.Set<Entities.Actors.Enemies.Spider>("spider", "ENEMIES");
    AssetManagement.Set<Entities.Actors.Enemies.Bat>("bat", "ENEMIES");
    
    this.MainScene.SetLevel(1);
}
```

You need to use this directory structure 
```
.
├──  Content
│   └── Maps
│         └──level_1.json
│         └──level_2.json
│   └── Sprites
│         └──tilemap.png
```
But you can change the defaults setting using the code below:
``` csharp
this.MainScene.MapLevelPath = "Maps/level_";
this.MainScene.TileMapPath = "Sprites/tilemap";
``` 