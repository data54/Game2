using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Player player1 = new Player();
        Vector2 playerPosition;
        KeyboardState currentKS;
        KeyboardState previousKS;
        SpriteFont font;

        Texture2D mainBackground;

        SpriteBatch spriteBatch;

        public Game1()
        {
            World.graphics = new GraphicsDeviceManager(this);
            World.graphics.PreferredBackBufferHeight = 800;
            World.graphics.PreferredBackBufferWidth = 800;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            this.playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X+GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("character2");
            playerAnimation.Initialize(playerTexture, new Vector2(0,100), 32, 48, 1, 50, Color.White, 1f, true);
            player1.Initialize(playerAnimation, playerPosition, 2, 1, 100, .3f);

            Textures.BlockTextures = Content.Load<Texture2D>("Blocks");
            Textures.LoadTextures();

            //World.Map.Initialize(2, 0, 5, 2);
            World.Map.GenerateBlock(0, 0, 15, 1);
            World.Map.GenerateBlock(1, 2, 1, 1);
            World.Map.GenerateBlock(3, 4, 1, 1);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            previousKS = currentKS;
            currentKS = Keyboard.GetState();
            
            // TODO: Add your update logic here

            UpdatePlayer(gameTime);
            base.Update(gameTime);
        }


        private void UpdatePlayer(GameTime gameTime)
        {
            player1.Update(gameTime);

            //foreach (KeyValuePair<Vector2, Block> pair in World.Map.Blocks)
            //{
            //    Rectangle block = new Rectangle((int)pair.Value.Location.X, (int)pair.Value.Location.Y, (int)World.BlockSize, (int)World.BlockSize);
            //    //if (MovementCollision.isOnTop(player1.rectangle, block))
            //    //{
            //        MovementCollision.CollideCheck(player1, block);
            //    //    player1.Jumping = false;
            //    //    player1.velocity.Y = 0;
            //    //}
            //}


            player1.Position.X = MathHelper.Clamp(player1.Position.X, 0, GraphicsDevice.Viewport.TitleSafeArea.Width - player1.Width);
            player1.Position.Y = MathHelper.Clamp(player1.Position.Y, 0, GraphicsDevice.Viewport.TitleSafeArea.Height - player1.Height);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            player1.Draw(spriteBatch);
            World.Map.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    public static class MovementCollision
    {
        const int margin = 5;
        public static bool isOnTop(this Rectangle r1, Rectangle r2)
        {
            return (r1.Bottom >= r2.Top - margin && r1.Bottom <= r2.Top && r1.Left <= r2.Right && r1.Right >=r2.Left );
        }
        public static void CollideCheck(Player player, Rectangle r1)
        {
            Rectangle pr = player.rectangle;

            if(pr.Intersects(r1))
            {
                if (pr.Bottom != r1.Top && player.Jumping)
                {
                    if ((pr.Bottom <= r1.Bottom + margin && pr.Bottom > r1.Top) && player.velocity.Y>=0)
                    {
                        if ((pr.Right >= r1.Left && pr.Left < r1.Right))
                        {
                            player.Jumping = false;
                            player.JumpLength = 0;
                            player.velocity.Y = 0;
                            player.updateY((int)player.Cordinants.Y);
                        }
                    }
                    else
                    {
                        //player.Jumping = false;
                        //player.velocity.Y = 0;
                    }
                }
            }

            //if(pr.Left >= r1.Left && (pr.Top<=r1.Top && pr.Top

            //if(pr.Bottom >= r1.Top-margin && pr.Bottom <= r1.Top && pr.Left<=
        }
    }
}
