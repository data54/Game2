using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Game2
{
    public class Mob
    {
        public Color[] textureData;

        public Texture2D Texture { get; set; }
        public Animation animation { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 Position;
        public Vector2 Cordinants
        {
            get
            {
                return new Vector2(Convert.ToInt32(Position.X / World.BlockSize), Convert.ToInt32(((World.graphics.GraphicsDevice.Viewport.Height - this.Position.Y) - this.Height) / World.BlockSize));
            }
            set
            {
                Position = new Vector2(value.X * World.BlockSize, World.graphics.GraphicsDevice.Viewport.Height - (value.Y * World.BlockSize));
            }
        }
        public int BlockHeight
        {
            get
            {
                return Height / World.BlockSize;
            }
        }
        public int BlockWidth
        {
            get
            {
                return Width / World.BlockSize;
            }
        }
        public bool Active { get; set; }
        public int Health { get; set; }
        public Direction Facing { get; set; }
        public float Speed { get; set; }
        public bool Moving { get; set; }
        public bool Jumping { get; set; }
        public Vector2 velocity = new Vector2();
        public float JumpHeight { get; set; }
        public int JumpLength { get; set; }
        public Rectangle rectangle
        {
            get
            {
                return this.animation.destinationRectangle;
            }
        }

        List<Vector2> nextCollision;


        public void Initialize(Animation _animation, Vector2 position, int height, int width, int health, float speed)
        {
            this.animation = _animation;
            this.Position = position;
            this.Active = true;
            this.Health = health;
            this.Speed = speed * World.BlockSize;
            this.Height = this.animation.Height = World.BlockSize * height;
            this.Width = this.animation.Width = World.BlockSize * width;

            this.Moving = false;
            this.Facing = Direction.Right;
            this.JumpHeight = 20f;
        }
        public void Update(GameTime gameTime)
        {
            animation.Position = new Vector2(Position.X-this.Width/2, Position.Y-this.Height/2);
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.animation.Draw(spriteBatch);
        }

        public void updateY(int y)
        {
            this.Position.Y = World.graphics.GraphicsDevice.Viewport.Height - (y * World.BlockSize) - this.Height;
        }

        public void updateX(int x)
        {
            this.Position.X = x * World.BlockSize;
        }

        public bool CanMoveLeft()
        {
            List<Vector2> list = new List<Vector2>();
            int x = Convert.ToInt32(Math.Ceiling(this.Position.X / World.BlockSize));
            int y = (World.graphics.GraphicsDevice.Viewport.Height / World.BlockSize) - Convert.ToInt32(Math.Floor(this.Position.Y / World.BlockSize)) - this.Height / World.BlockSize;

            for (int i = 0; i < this.Height / World.BlockSize; i++)
            {
                list.Add(new Vector2(x - 1, y + i));
            }

            if (World.Passable(list))
            {
                return true;
            }
            else return false;
        }
        public bool CanMoveRight()
        {
            List<Vector2> list = new List<Vector2>();
            int x = Convert.ToInt32(Math.Floor(this.Position.X / World.BlockSize));
            int y = (World.graphics.GraphicsDevice.Viewport.Height / World.BlockSize) - Convert.ToInt32(Math.Floor(this.Position.Y / World.BlockSize)) - this.Height / World.BlockSize;

            for (int i = 0; i < this.Height / World.BlockSize; i++)
            {
                list.Add(new Vector2(this.Cordinants.X + 1, this.Cordinants.Y + i));
            }

            if (World.Passable(list))
            {
                return true;
            }
            else return false;
        }

        public bool Falling()
        {
            List<Vector2> list = new List<Vector2>();
            int x1 = (int)this.Cordinants.X;
            int x2 = x1 + this.Width / World.BlockSize;
            int y = Convert.ToInt32(((World.graphics.GraphicsDevice.Viewport.Height - this.Position.Y)-this.Height) / World.BlockSize);
            int y2 = Convert.ToInt32(((World.graphics.GraphicsDevice.Viewport.Height - this.Position.Y) - (this.Height + this.velocity.Y)) / World.BlockSize);
            

            for (int i = 0; i <= x2 - x1; i++)
            {
                list.Add(new Vector2(x1 + i, y - 1));
            }

            if (this.Jumping)
            {
                this.velocity.Y += 0.05f * this.JumpLength;
                this.JumpLength++;
            }

            if (World.Passable(list))
            {
                if (velocity.Y <= 0)
                {
                    this.Jumping = true;
                }
                if (nextCollision == null)
                {
                    nextCollision = list;
                }
                return true;
            }

            if (velocity.Y > 0 && nextCollision == null && list[0].Y >= 0)
            {
                nextCollision = list;
            }

            nextCollision = new List<Vector2>();

            for (int i = 0; i < this.Width / World.BlockSize; i++)
            {
                for (int k = 0; k < this.Height / World.BlockSize; k++)
                {
                    nextCollision.Add(new Vector2(this.Cordinants.X+i,this.Cordinants.Y+k));
                }
            }

            if (velocity.Y > 0 && World.Intersects(this.rectangle, nextCollision))
            {
                this.Jumping = false;
                this.velocity.Y = 0;
                this.JumpLength = 0;
                foreach (Vector2 vec in nextCollision)
                {
                    if (World.Map.Blocks.ContainsKey(vec))
                    {
                        this.Position.Y = World.Map.Blocks[vec].rectangle.Top - this.Height;
                        break;
                    }
                }
                nextCollision = null;
            }

            return false;
        }


        Vector2 ct = new Vector2(1, 2);
        public void CollisionTest()
        {
            if (Collision.PixelCollision(this.rectangle, this.animation.TextureData, World.Map.Blocks[ct].rectangle, Textures.BlockTextureData(0)))
            {
                //World.graphics.GraphicsDevice.Clear(Color.Red);
            }
            else
            {
                //World.graphics.GraphicsDevice.Clear(Color.Blue);
            }
        }

        public void MoveLeft()
        {
            if (this.CanMoveLeft())
            {
                this.velocity.X = -1 * this.Speed;
            }
            else
            {
                this.velocity.X = 0;
            }

            this.Facing = Direction.Left;
        }
        public void MoveRight()
        {
            if (this.CanMoveRight())
            {
                this.velocity.X = 1 * this.Speed;
            }
            else
            {
                this.velocity.X = 0;
            }
            this.Facing = Direction.Right;
        }
        public void Jump()
        {
            this.JumpLength = 0;
            this.Jumping = true;
            this.Position.Y -= this.JumpHeight;
            this.velocity.Y = -1 * World.Gravity;
        }
    }

    public class Player : Mob
    {
        //public Player(ContentManager content)
        //{
        //    Texture = content.Load<Texture2D>("character2");
        //    textureData = new Color[Texture.Width * Texture.Height];
        //    Texture.GetData(textureData);
        //}

        new public void Update(GameTime gameTime)
        {
            this.Position += velocity;
            KeyboardState kb = Keyboard.GetState();

            if (!this.Falling())
            {
                if (kb.IsKeyDown(Keys.Space))
                {
                    this.Jump();
                }
            }

            if (kb.IsKeyDown(Keys.A))
            {
                MoveLeft();
            }
            else if (kb.IsKeyDown(Keys.D))
            {
                MoveRight();
            }
            else
            {
                this.velocity.X = 0;
            }


            //if (this.Position.Y + this.Height > World.graphics.GraphicsDevice.Viewport.Height)
            //{
            //    Jumping = false;

            //    updateY(0);
            //    this.velocity.Y = 0;
            //}


            //else if (kb.IsKeyDown(Keys.S))
            //{
            //    moveDirection.Y = 1 * this.Speed;
            //}

            if (velocity == Vector2.Zero)
            {
                this.Moving = false;
            }
            else
            {
                this.Moving = true;
            }


            //this.Position.X = MathHelper.Clamp(this.Position.X, 0, GraphicsDevice.Viewport.TitleSafeArea.Width - this.Width);
            //this.Position.Y = MathHelper.Clamp(this.Position.Y, 0, GraphicsDevice.Viewport.TitleSafeArea.Height - this.Height);
            animation.Position = new Vector2(Position.X - this.Width / 2, Position.Y - this.Height / 2);
            animation.Update(gameTime);
            CollisionTest();
        }
    }
}
