#region Using Statements
using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Pong {

    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const int Height = 600;
        private const int Width = 800;

        private const int BarToEdgePadding = 40;

        private Bar playerBar;
        private Bar cpuBar;
        private BallOutMananger ballOutManager;

        private CpuController cpuController;
        private Score score;

        public Game1(): base() 
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = Height;
            graphics.PreferredBackBufferWidth = Width;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "Content";
            var barTexture = Content.Load<Texture2D>("PongBar");
            var ballTexture = Content.Load<Texture2D>("PongBall");

            var ballOutSound = Content.Load<SoundEffect>("ballOutSound");
            var ballCollideWithBarSound = Content.Load<SoundEffect>("ballBarCollision");
            var ballCollideWithEdgeSound = Content.Load<SoundEffect>("ballEdgeCollision");

            var music = Content.Load<SoundEffect>("music");
            SoundEffectInstance soundEffectInstance = music.CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();

            var font = Content.Load<SpriteFont>("Score");

            playerBar = new Bar(barTexture, BarToEdgePadding, Height, ballCollideWithBarSound);
            cpuBar = new Bar(barTexture, Width - BarToEdgePadding, Height, ballCollideWithBarSound);

            var ball = new Ball(ballTexture, Width / 2, Width, Height, ballCollideWithEdgeSound);

            score = new Score(font);
            ballOutManager = new BallOutMananger(ball, Width, ballOutSound, score);

            cpuController = new CpuController(cpuBar, ball, Height);

        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (score.IsGameFinished()) {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                    score.Reset();
                    playerBar.ResetHorizontalMovementSpeed();
                    cpuBar.ResetHorizontalMovementSpeed();
                }
                return;
            }

            var keyboardState = Keyboard.GetState();
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Up) || gamePadState.DPad.Up == ButtonState.Pressed)
            {
                playerBar.MoveUp();
            }

            if (keyboardState.IsKeyDown(Keys.Down) || gamePadState.DPad.Down == ButtonState.Pressed)
            {
               playerBar.MoveDown();
            }

            if (ballOutManager.IsBallOut()) {
                ballOutManager.ResetBallAfterLatency();
                ballOutManager.GetBall().ResetHorizontalMovement();
                playerBar.ResetHorizontalMovementSpeed();
                cpuBar.ResetHorizontalMovementSpeed();
            }
            else 
            {
                ballOutManager.Move();
                ballOutManager.GetBall().CheckHit(playerBar, cpuBar);
                cpuController.UpdatePosition(); 
            }
            base.Update(gameTime);
        }

        private void GameOver()
        {
            DisplayEndMessage("Game Over");
        }

        private void Win()
        {
            DisplayEndMessage("You  Win");
        }

        private void DisplayEndMessage(String message)
        {
            spriteBatch.DrawString(score.GetFont(), message, new Vector2(40, Height / 2), Color.White, 0f, new Vector2(0, 0), new Vector2(3, 3), SpriteEffects.None, 0f);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // Draw player and cpu bars
            spriteBatch.Draw(playerBar.GetTexture(), new Rectangle(playerBar.GetXposition(), playerBar.GetYposition(), Bar.Width, Bar.Height), Color.White);
            spriteBatch.Draw(cpuBar.GetTexture(), new Rectangle(cpuBar.GetXposition(), cpuBar.GetYposition(), Bar.Width, Bar.Height), Color.White);

            // Draw the ball
            spriteBatch.Draw(ballOutManager.GetBall().GetTexture(), new Rectangle(ballOutManager.GetBall().GetXposition(), ballOutManager.GetBall().GetYposition(), 8, 8), Color.White);

            // Draw the Scrore values
            spriteBatch.DrawString(score.GetFont(), score.GetPlayerScore().ToString(CultureInfo.InvariantCulture), new Vector2(Width / 2 - 40 - 15, 20), Color.White);
            spriteBatch.DrawString(score.GetFont(), score.GetCpuScore().ToString(CultureInfo.InvariantCulture), new Vector2(Width / 2 + 40, 20), Color.White);

            DrawNet();

            if (score.PlayerWin())
            {
                Win();
            }

            if (score.CpuWin())
            {
                GameOver();
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }

        private void DrawNet()
        {
            for (var yPosition = 20; yPosition < Height - BarToEdgePadding; yPosition += 35)
            {
                spriteBatch.Draw(playerBar.GetTexture(), new Rectangle(Width / 2, yPosition, 5, 30), Color.White);
            }
        }
    }
}
