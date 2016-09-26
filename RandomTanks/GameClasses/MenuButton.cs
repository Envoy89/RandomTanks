using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTanks.GameClasses
{
    class MenuButton
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        Color color = new Color(255, 255, 255, 255);
        Vector2 size;
        bool down;
        public bool isClicked;

        public MenuButton(Texture2D texture, GraphicsDevice graphicsDevice)
        {
            this.texture = texture;
            this.size = new Vector2(graphicsDevice.Viewport.Width / 8, graphicsDevice.Viewport.Height / 20);
        }

        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if(rectangle.Intersects(mouseRect))
            {
                if(color.A == 255)
                {
                    down = true;
                }
                if(color.A == 0)
                {
                    down = false;
                }
                if (down) { color.A -= 3; } else { color.A += 3; }
                if(mouse.LeftButton == ButtonState.Pressed) { isClicked = true; }
                else { isClicked = false; }
            }
            else if (color.A < 255)
            {
                color.A += 3;
                isClicked = false;
            }
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
