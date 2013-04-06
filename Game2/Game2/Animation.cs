using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    public class Animation
    {
        Texture2D textureStrip;
        List<Color[]> TextureDataList = new List<Color[]>();
        float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;
        Color color;
        Rectangle sourceRectangle;
        public Rectangle destinationRectangle;
        public int FrameWidth, FrameHeight;
        public bool Active, Looping;
        public Vector2 Position;
        public int Width { get; set; }
        public int Height { get; set; }
        public Color[] TextureData
        {
            get
            {
                return TextureDataList[currentFrame];
            }
        }

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            textureStrip = texture;

            int elements = frameWidth * frameHeight;
            for (int i = 0; i < frameCount; i++)
            {
                Color[] temp = new Color[elements];
                textureStrip.GetData(0, new Rectangle(i*frameWidth, i*frameHeight, frameWidth, frameHeight), temp, i*elements,elements);
                TextureDataList.Add(temp);
            }

            elapsedTime = 0;
            currentFrame = 0;

            Active = true;
        }
        public void Update(GameTime gameTime)
        {
            if (Active == false)
                return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;


            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;


                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRectangle = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRectangle = new Rectangle((int)Position.X,
            (int)Position.Y,
            (int)(Width),
            (int)(Height));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                spriteBatch.Draw(textureStrip, destinationRectangle, sourceRectangle, color);
            }
        }
    }
}
