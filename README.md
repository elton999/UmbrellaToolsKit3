# UmbrellaToolsKit
## About
UmbrellaToolsKit is a game framework made with [MonoGame](https://www.monogame.net) for personal uses, however you can feel free to use it.

## Get Started
### Content pipeline

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
### Behaviors Trees
### Coroutines

## Particles System

## Storage

# How to build