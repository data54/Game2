using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    public class Block
    {
        public int ID { get; set; }

        public Vector2 Location { get; set; }
        public Vector2 Coordinants
        {
            get
            {
                return new Vector2(Location.X / World.BlockSize, (World.graphics.PreferredBackBufferHeight/World.BlockSize) - ((int) Location.Y / World.BlockSize)-1);
            }
        }
        public bool Solid { get; set; }
        public int Health { get; set; }
        private Rectangle sourceRectangle;

        public Rectangle rectangle
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y - World.BlockSize, World.BlockSize, World.BlockSize);
            }
        }

        public Block(Vector2 location, int id)
        {
            Location = new Vector2(location.X*World.BlockSize, (int) World.graphics.GraphicsDevice.Viewport.Height- location.Y*World.BlockSize);
            sourceRectangle = Textures.BlockTexture(id);
            Solid = true;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.BlockTextures, rectangle, sourceRectangle, Color.Yellow);
        }
    }
}
