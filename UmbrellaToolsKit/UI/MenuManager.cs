using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UmbrellaToolsKit.UI
{
    public class MenuManager : GameObject
    {
        public SpriteFont Font;
        public List<string> MenuItems = new List<string>();
        public List<Vector2> MenuItensSizes = new List<Vector2>();
        public List<Vector2> MenuItensPositions = new List<Vector2>();

        private int ItemOver;
        public int ItemSelected;

        private bool IsPressed = false;

        public override void Update(GameTime gameTime)
        {
            // move up item
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !this.IsPressed)
            {
                this.IsPressed = true;
                if (this.ItemOver > 0) this.ItemOver--;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Up)) this.IsPressed = false;

            // move down item
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !this.IsPressed)
            {
                this.IsPressed = true;
                if (this.ItemOver < this.MenuItems.Count) this.ItemOver++;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down)) this.IsPressed = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
