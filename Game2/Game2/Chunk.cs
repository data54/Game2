using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    public class Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Coordinants
        {
            get
            {
                return new Vector2(X / World.BlockSize, (World.graphics.PreferredBackBufferHeight / World.BlockSize) - (Y / World.BlockSize));
            }
        }

        public int Height {get; set;}
        public int Width {get; set;}
        public Dictionary<Vector2, Block> Blocks = new Dictionary<Vector2, Block>();

        public void GenerateBlock(int x, int y, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Blocks.Add(new Vector2((float)i + x, y + j), new Block(new Vector2((float)i + x, y + j), 1));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<Vector2, Block> pair in Blocks)
            {
                pair.Value.Draw(spriteBatch);                
            }
        }
    }
}
