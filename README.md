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
        scene.Middleground.AddGameObject(gameObj, Layers.MIDDLEGROUND);
        //...
    }
}
```

### Render an sprite
<img src="https://raw.githubusercontent.com/elton999/UmbrellaToolsKit3/main/Concept/atlas_editor.png" width="100%" />

```csharp
var sprite = new GameObject();
            _gameManagement.SceneManagement.MainScene.AddGameObject(sprite, Layers.MIDDLEGROUND);
            sprite.AddComponent<SpriteComponent>().SetAtlas("Sprites/boss : 0");
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

Creating a new component:

```csharp
using System;
using UmbrellaToolsKit;

namespace Project.Components
{
    public class HealthComponent : Component
    {
        [ShowEditor] private bool _isImmortal = false;

        [ShowEditor] public float HP = 10.0f;
        public bool IsAlive => HP > 0.0f;
        public static event Action<GameObject> OnAnyEntityDie;

        public event Action OnDie;

        public void TakeDamage(float damage)
        {
            if (!IsAlive || _isImmortal) return;

            HP = Math.Clamp(HP - damage, 0, float.PositiveInfinity);

            if (HP > 0.0f) return;

            OnAnyEntityDie?.Invoke(GameObject);
            OnDie?.Invoke();
        }

        public void BeImmortal() => _isImmortal = true;
    }
}
```

You can add your own components to any game object:

```csharp
namespace Project
{
    public class Game1 : Game
    {
        //...
        var scene = _gameManagement.SceneManagement.MainScene;
        var gameObj = new GameObject();
        scene.Middleground.AddGameObject(gameObj, Layers.MIDDLEGROUND);
        gameObj.AddComponent<HealthComponent>();
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