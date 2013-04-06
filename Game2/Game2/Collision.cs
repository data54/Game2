using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    public static class Collision
    {
        static RenderTarget2D renderTarget;
        static RenderTarget2D rotatedrenderTarget;
        static SpriteBatch spritebatch = new SpriteBatch(World.graphics.GraphicsDevice);

        public static bool PixelCollision(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            if (rect1.Intersects(rect2))
            {
                int top = Math.Max(rect1.Top, rect2.Top);
                int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
                int left = Math.Max(rect1.Left, rect2.Left);
                int right = Math.Min(rect1.Right, rect2.Right);

                renderTarget = new RenderTarget2D(World.graphics.GraphicsDevice, rect1.Width, rect1.Height, true, SurfaceFormat.Color, DepthFormat.Depth24);


                for (int y = top; y < bottom; y++)
                {
                    for (int x = left; x < right; x++)
                    {
                        Color c1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                        Color c2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                        if (c1.A != 0 && c2.A != 0) return true;
                    }
                }
            }
            return false;
        }

        private static Texture2D CreateCollisionTexture(Rectangle rect1, Rectangle rect2)
        {
            World.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            World.graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);

            spritebatch.Begin();
            spritebatch.Draw(Textures.BlockTextures, rect2, Textures.BlockTexture(1), Color.White);
            spritebatch.Draw(Textures.BlockTextures, rect2, Textures.BlockTexture(1), Color.White);
            spritebatch.End();
                        
            World.graphics.GraphicsDevice.SetRenderTarget(null);

            return renderTarget;
        }
    }
}
