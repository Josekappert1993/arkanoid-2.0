using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bgTexture;
        Paddle paddle;  // player 1
        Paddle paddle2; // player 2
        List<Ball> balls = new List<Ball>();

        SoundEffect ballBounceSFX;
        SoundEffect ballHitSFX;
        SoundEffect deathSFX;
        SoundEffect powerupSFX;

        List<Block> blocks = new List<Block>();

        List<PowerUp> powerups = new List<PowerUp>();

        Level level;
        int levelNumber = 1;

        Random random = new Random();
        double powerUpChance = 0.2;

        bool ballCatchActive = false;
        bool ballCatchActive2 = false;

        //player 1
        int score = 0;
        int lives = 3;

        //player 2
        int score2 = 0;
        int lives2 = 3;

        bool gameOver = false;
        int pointsTilLife = 20000;

        int speedMult = 0;

        SpriteFont font;

        bool inLevelBreak = false;
        float levelBreakTime = 0.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
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
            bgTexture = Content.Load<Texture2D>("bg");
            font = Content.Load<SpriteFont>("main_font");

            ballBounceSFX = Content.Load<SoundEffect>("ball_bounce");
            ballHitSFX = Content.Load<SoundEffect>("ball_hit");
            deathSFX = Content.Load<SoundEffect>("death");
            powerupSFX = Content.Load<SoundEffect>("powerup");

            paddle = new Paddle(Keys.Left,Keys.Right,this);
            paddle2 = new Paddle(Keys.A, Keys.D,this);

            paddle.LoadContent();
            paddle2.LoadContent();

            paddle.position = new Vector2(512, 740);
            paddle2.position = new Vector2(512, 28);
            
            LoadLevel("Level1.xml");
            StartLevelBreak();
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If we're in a break or the game is over, stop updating objects
            if (!inLevelBreak && !gameOver)
            {
                // Figure out how much the paddle moved
                float oldX = paddle.position.X;
                paddle.Update(deltaTime);
                float paddleOffset = paddle.position.X - oldX;

                float oldX2 = paddle2.position.X;
                paddle2.Update(deltaTime);
                float paddleOffset2 = paddle2.position.X - oldX;

                // Update balls
                foreach (Ball b in balls)
                {
                    if (b.caught)
                    {
                        b.position.X += paddleOffset;
                    }

                    if (b.caught)
                    {
                        b.position.X += paddleOffset2;
                    }

                    b.Update(deltaTime);
                    CheckCollisions(b);
                }

                // Check if we should remove any balls
                RemoveBalls();

                // Update powerups
                foreach (PowerUp p in powerups)
                {
                    p.Update(deltaTime);
                }

                CheckForPowerups();
                RemovePowerUps();

                // Check if it's time to move to the next level
                if (blocks.Count == 0)
                {
                    NextLevel();
                }
            }
            else if (!gameOver)
            {
                levelBreakTime -= deltaTime;
                // Is the break over?
                if (levelBreakTime <= 0)
                {
                    inLevelBreak = false;
                    SpawnBall();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SandyBrown);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // Draw all sprites here
            spriteBatch.Draw(bgTexture, new Vector2(0, 0), Color.White);
            paddle.Draw(spriteBatch);

            paddle2.Draw(spriteBatch);

            foreach (Ball b in balls)
            {
                b.Draw(spriteBatch);
            }
            foreach (Block b in blocks)
            {
                b.Draw(spriteBatch);
            }
            foreach (PowerUp p in powerups)
            {
                p.Draw(spriteBatch);
            }
            spriteBatch.DrawString(font, String.Format("Score: {0:#,###0}", score),
                new Vector2(40, 10), Color.White);

            string livesText = String.Format("Lives: {0}", lives);
            Vector2 strSize = font.MeasureString(livesText);
            Vector2 strLoc = new Vector2(984, 10);
            strLoc.X -= strSize.X;
            spriteBatch.DrawString(font, livesText, strLoc, Color.White);

            if (inLevelBreak)
            {
                string levelText = String.Format("Level {0}", levelNumber);
                strSize = font.MeasureString(levelText);
                strLoc = new Vector2(1024 / 2, 768 / 2);
                strLoc.X -= strSize.X / 2;
                strLoc.Y -= strSize.Y / 2;
                spriteBatch.DrawString(font, levelText, strLoc, Color.White);
            }

            // Game over text
            if (gameOver)
            {
                string gameOverText = "Game Over";
                strSize = font.MeasureString(gameOverText);
                strLoc = new Vector2(1024 / 2, 768 / 2);
                strLoc.X -= strSize.X / 2;
                strLoc.Y -= strSize.Y / 2;
                spriteBatch.DrawString(font, gameOverText, strLoc, Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);

        }

        protected void CheckCollisions(Ball ball)
        {
            // Don't collide balls that are currently caught
            if (ball.caught)
            {
                return;
            }
            float radius = ball.Width / 2;

            // Check for paddle
            if (ball.withPaddle == 0 &&
                (ball.position.X > (paddle.position.X - radius - paddle.Width / 2)) &&
                (ball.position.X < (paddle.position.X + radius + paddle.Width / 2)) &&
                (ball.position.Y < paddle.position.Y) &&
                (ball.position.Y > (paddle.position.Y - radius - paddle.Height / 2)))
            {
                // Reflect based on which part of the paddle is hit

                // By default, set the normal to "up"
                Vector2 normal = -1.0f * Vector2.UnitY;

                // Distance from the leftmost to rightmost part of the paddle
                float dist = paddle.Width + radius * 2;
                // Where within this distance the ball is at
                float ballLocation = ball.position.X -
                    (paddle.position.X - radius - paddle.Width / 2);
                // Percent between leftmost and rightmost part of paddle
                float pct = ballLocation / dist;

                if (pct < 0.33f)
                {
                    normal = new Vector2(-0.196f, -0.981f);
                }
                else if (pct > 0.66f)
                {
                    normal = new Vector2(0.196f, -0.981f);
                }

                ball.direction = Vector2.Reflect(ball.direction, normal);

                // Fix up the direction if it's too steep
                float dotResult = Vector2.Dot(ball.direction, Vector2.UnitX);
                if (dotResult > 0.9f)
                {
                    ball.direction = new Vector2(0.906f, -0.423f);
                }
                dotResult = Vector2.Dot(ball.direction, -Vector2.UnitX);
                if (dotResult > 0.9f)
                {
                    ball.direction = new Vector2(-0.906f, -0.423f);
                }
                dotResult = Vector2.Dot(ball.direction, -Vector2.UnitY);
                if (dotResult > 0.9f)
                {
                    // We need to figure out if we're clockwise or counter-clockwise
                    Vector3 crossResult = Vector3.Cross(new Vector3(ball.direction, 0),
                        -Vector3.UnitY);
                    if (crossResult.Z < 0)
                    {
                        ball.direction = new Vector2(0.423f, -0.906f);
                    }
                    else
                    {
                        ball.direction = new Vector2(-0.423f, -0.906f);
                    }
                }
                
                // No collisions between ball and paddle for 20 frames
                ball.withPaddle = 20;
                ballBounceSFX.Play();

                if (ballCatchActive)
                {
                    ball.caught = true;
                    if (pct < 0.5f)
                    {
                        ball.direction = new Vector2(-0.707f, -0.707f);
                    }
                    else
                    {
                        ball.direction = new Vector2(0.707f, -0.707f);
                    }
                }
            }

            // Check for blocks
            // First, let's see if we collided with any block
            Block collidedBlock = null;
            foreach (Block b in blocks)
            {
                if ((ball.position.X > (b.position.X - b.Width / 2 - radius)) &&
                    (ball.position.X < (b.position.X + b.Width / 2 + radius)) &&
                    (ball.position.Y > (b.position.Y - b.Height / 2 - radius)) &&
                    (ball.position.Y < (b.position.Y + b.Height / 2 + radius)))
                {
                    collidedBlock = b;
                    break;
                }
            }

            // Now figure out how to reflect the ball
            if (collidedBlock != null)
            {
                // Assume that if our Y is close to the top or bottom of the block,
                // we're colliding with the top or bottom
                if ((ball.position.Y <
                    (collidedBlock.position.Y - collidedBlock.Height / 2)) ||
                    (ball.position.Y >
                    (collidedBlock.position.Y + collidedBlock.Height / 2)))
                {
                    ball.direction.Y = -1.0f * ball.direction.Y;
                }
                else // otherwise, we have to be colliding from the sides
                {
                    ball.direction.X = -1.0f * ball.direction.X;
                }

                // Now remove this block from the list, if we should
                bool shouldRemove = collidedBlock.OnHit();
                if (shouldRemove)
                {
                    blocks.Remove(collidedBlock);
                    // Check if we should spawn a power-up
                    if (random.NextDouble() < powerUpChance)
                    {
                        SpawnPowerUp(collidedBlock.position);
                    }

                    // Add points
                    AddScore(100 + 100 * speedMult);
                }

                ballHitSFX.Play();
            }

            // Check walls
            if (Math.Abs(ball.position.X - 32) < radius)
            {
                ball.direction.X = -1.0f * ball.direction.X;
                ballBounceSFX.Play();
            }
            else if (Math.Abs(ball.position.X - 992) < radius)
            {
                ball.direction.X = -1.0f * ball.direction.X;
                ballBounceSFX.Play();
            }
            else if (Math.Abs(ball.position.Y - 32) < radius)
            {
                ball.direction.Y = -1.0f * ball.direction.Y;
                ballBounceSFX.Play();
            }
            else if (ball.position.Y > (768 + radius))
            {
                ball.shouldRemove = true;
            }
        }

        protected void LoseLife()
        {
            // Reset paddle and ball
            paddle.position = new Vector2(512, 740);
            deathSFX.Play();

            // Disable powerups
            ballCatchActive = false;
            paddle.ChangeTexture("paddle");

            if (lives > 0)
            {
                lives--;
                SpawnBall();
            }
            else
            {
                gameOver = true;
            }
        }

        protected void SpawnPowerUp(Vector2 position)
        {
            int type = random.Next(3);
            PowerUp p = new PowerUp((PowerUpType)type, this);
            p.LoadContent();
            p.position = position;
            powerups.Add(p);
        }

        protected void RemovePowerUps()
        {
            for (int i = powerups.Count - 1; i >= 0; i--)
            {
                if (powerups[i].shouldRemove)
                {
                    powerups.RemoveAt(i);
                }
            }
        }

        protected void CheckForPowerups()
        {
            Rectangle paddleRect = paddle.BoundingRect;
            foreach (PowerUp p in powerups)
            {
                Rectangle powerupRect = p.BoundingRect;
                if (paddleRect.Intersects(powerupRect))
                {
                    p.shouldRemove = true;
                    powerupSFX.Play();
                    switch (p.type)
                    {
                        case (PowerUpType.BallCatch):
                            ballCatchActive = true;
                            break;
                        case (PowerUpType.PaddleSize):
                            paddle.ChangeTexture("paddle_long");
                            break;
                        case (PowerUpType.MultiBall):
                            SpawnBall();
                            break;
                    }

                    // Add points
                    AddScore(500 + 500 * speedMult);
                }
            }
        }

        protected void SpawnBall()
        {
            Ball ball = new Ball(this);
            ball.LoadContent();
            ball.position = paddle.position;
            ball.position.Y -= ball.Height + paddle.Height;
            ball.speed = level.ballSpeed + 100 * speedMult;
            balls.Add(ball);
        }

        protected void RemoveBalls()
        {
            for (int i = balls.Count - 1; i >= 0; i--)
            {
                if (balls[i].shouldRemove)
                {
                    balls.RemoveAt(i);
                }
            }

            if (balls.Count == 0)
            {
                LoseLife();
            }
        }

        protected void LoadLevel(string levelName)
        {
            using (FileStream fs = File.OpenRead(@"Assets/Levels/" + levelName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Level));
                level = (Level)serializer.Deserialize(fs);
            }

            // Generate blocks based on level.layout array
            for (int i = 0; i < level.layout.Length; i++)
            {
                for (int j = 0; j < level.layout[i].Length; j++)
                {
                    if (level.layout[i][j] != 9)
                    {
                        Block tempBlock = new Block((BlockColor)level.layout[i][j], this);
                        tempBlock.LoadContent();
                        tempBlock.position = new Vector2(64 + j * 64, 300 + i * 32);
                        blocks.Add(tempBlock);
                    }
                }
            }
        }

        protected void NextLevel()
        {
            // Add points
            AddScore(5000 + 5000 * speedMult + 500 * (balls.Count - 1) * speedMult);

            balls.Clear();
            powerups.Clear();

            paddle.position = new Vector2(512, 740);
            StartLevelBreak();

            // Disable powerups
            ballCatchActive = false;
            paddle.ChangeTexture("paddle");

            if (level.nextLevel == "Level1.xml")
            {
                speedMult++;
            }
            LoadLevel(level.nextLevel);

            levelNumber++;
        }

        protected void AddScore(int value)
        {
            score += value;

            pointsTilLife -= value;
            bool gainedLife = false;
            // Do a while loop here in case we gain multiple lives,
            // which could happen if the player gets a really big end level bonus
            while (pointsTilLife <= 0)
            {
                lives++;
                pointsTilLife += 20000;
                gainedLife = true;
            }

            if (gainedLife)
            {
                powerupSFX.Play();
            }
        }

        protected void StartLevelBreak()
        {
            inLevelBreak = true;
            levelBreakTime = 2.0f;
        }

    }
}
