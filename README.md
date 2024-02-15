# UmbrellaToolsKit
## About
UmbrellaToolsKit is a game framework made with [MonoGame](https://www.monogame.net) for personal uses, however you can feel free to use it.
<img src="https://raw.githubusercontent.com/elton999/UmbrellaToolsKit3/main/Concept/opengl_f6IeN5LRbf.png" width="100%" />

## Get Started

### Content pipeline

### Inputs
```csharp
namespace Project
{
    public class Game1 : Game
    {
        
        //...
        protected override void Initialize()
        {
            // inputs
            KeyBoardHandler.AddInput("interact", new Keys[] { Keys.Enter, Keys.X, Keys.Space });
            KeyBoardHandler.AddInput("exit", Keys.Escape);

            KeyBoardHandler.AddInput("up", new Keys[] { Keys.W, Keys.Up });
            KeyBoardHandler.AddInput("down", new Keys[] { Keys.S, Keys.Down });
            KeyBoardHandler.AddInput("left", new Keys[] { Keys.A, Keys.Left });
            KeyBoardHandler.AddInput("right", new Keys[] { Keys.D, Keys.Right });

            base.Initialize();
        }
        //...
    }
}
```

## Sprites
### Render an image
```csharp
namespace Project
{
    public class Game1 : Game
    {
        //...
        var scene = _gameManagement.SceneManagement.MainScene;
        var gameObj = new GameObject();
        gameObj.Sprite = Content.Load<Texture2D>("Sprites/picture");
        scene.Middleground.Add(gameObj);
        //...
    }
}
```
### Aseprite
[Aseprite](https://www.aseprite.org) is a pixel art animation software. You can import its files using the steps below:

```csharp
using UmbrellaToolsKit;
using UmbrellaToolsKit.Sprite;
public class Player : GameObject
{
    //...
    public AsepriteAnimation Animation;

    public override void Start()
    {
        Animation = new AsepriteAnimation(Content.Load<AsepriteDefinitions>("Sprites/player_animation"));
        Sprite = Content.Load<Texture2D>("Sprites/player");
        //...
    }

    public override void Update(GameTime gameTime)
    {
        Animation.Play(gameTime, "walk", AsepriteAnimation.AnimationDirection.LOOP);
        Body = Animation.Body;
        //...
    }
    //...
}
```

## TileMaps
### LDTK
### OGMO Editor

## Physics
### Actors and Solids
### Grids

## Behaviors
### Components
<img src="https://raw.githubusercontent.com/elton999/UmbrellaToolsKit3/main/Concept/project-opengl_pOItuqnfwN.png" width="100%" />

```csharp
namespace Project
{
    public class Game1 : Game
    {
        //...
        var scene = _gameManagement.SceneManagement.MainScene;
        var gameObj = new GameObject();
        gameObj.Sprite = Content.Load<Texture2D>("Sprites/picture");
        scene.Middleground.Add(gameObj);
        //...
    }
}
```

### Behaviors Trees
### Coroutines
### Nodes
<img src="https://raw.githubusercontent.com/elton999/UmbrellaToolsKit3/main/Concept/project-opengl_l7uxFQIoFe.png" width="100%" />

## Particles System

## Storage

# How to build