using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    public static class Textures
    {
        public static Texture2D BlockTextures;
        static Dictionary<int, Rectangle> blocks = new Dictionary<int, Rectangle>();
        static Dictionary<int, Color[]> blockTextureData = new Dictionary<int, Color[]>();


        public static int BlockTextureSize = 16;

        public static void LoadTextures()
        {
            int width = BlockTextures.Width / BlockTextureSize;
            int height = BlockTextures.Height / BlockTextureSize;
            int textureDataElements = BlockTextureSize*BlockTextureSize;

            for (int i = 0; i < height; i++)//height
            {
                for (int j = 0; j < width; j++)//width
                {
                    Rectangle source = new Rectangle(j * BlockTextureSize, (i * BlockTextureSize), BlockTextureSize, BlockTextureSize);
                    blocks.Add((i * width) + j, source);
                    Color[] temp = new Color[textureDataElements];
                    BlockTextures.GetData(0, source, temp, 0, textureDataElements);
                    blockTextureData.Add((i * width) + j, temp);
                }
            }
        }

        public static Rectangle BlockTexture(int id)
        {
            if (blocks.ContainsKey(id))
            {
                return blocks[id];
            }
            return blocks[0];
        }

        public static Color[] BlockTextureData(int id)
        {
            if (blocks.ContainsKey(id))
            {
                return blockTextureData[id];
            }
            return null;
        }

    }
}
