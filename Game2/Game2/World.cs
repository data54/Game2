using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    public enum Direction { Left, Right };

    public static class World
    {
        public static int Scale = 3;
        public static int BlockInitialSize = 16;
        public static int BlockSize = Scale * BlockInitialSize;

        public static float Gravity = 10f;
        public static GraphicsDeviceManager graphics;
        public static Chunk Map = new Chunk();
        
        public static bool Passable(List<Vector2> list)
        {
            foreach (Vector2 block in list)
            {
                if ((Map.Blocks.ContainsKey(block) && Map.Blocks[block].Solid)||block.Y<0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Intersects(Rectangle player, List<Vector2> blocks)
        {
            foreach (Vector2 block in blocks)
            {
                if ((Map.Blocks.ContainsKey(block) && Map.Blocks[block].Solid) && player.Intersects(Map.Blocks[block].rectangle) || block.Y<0)
                {
                    return true;
                }
            }
            return false;
        }

        //public static bool isLanding(Rectangle player, List<Vector2> blocks)
        //{



        //    if (player.Intersects(r1))
        //    {
        //        if (pr.Bottom != r1.Top && player.Jumping)
        //        {
        //            if ((pr.Bottom <= r1.Bottom + margin && pr.Bottom > r1.Top) && player.velocity.Y >= 0)
        //            {
        //                if ((pr.Right >= r1.Left && pr.Left < r1.Right))
        //                {
        //                    player.Jumping = false;
        //                    player.JumpLength = 0;
        //                    player.velocity.Y = 0;
        //                    player.updateY((int)player.Cordinants.Y);
        //                }
        //            }
        //            else
        //            {
        //                //player.Jumping = false;
        //                //player.velocity.Y = 0;
        //            }
        //        }
        //    }
        //}



    }
}
