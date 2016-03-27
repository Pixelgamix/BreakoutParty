using BreakoutParty.Entities;
using BreakoutParty.Font;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Gamestates
{
    /// <summary>
    /// Breakout game state.
    /// </summary>
    sealed class BreakoutState : Gamestate
    {
        /// <summary>
        /// Font for displaying status information.
        /// </summary>
        private BitmapFont _Statsfont;

        /// <summary>
        /// Score.
        /// </summary>
        public int Score = 0;

        /// <summary>
        /// Position of score display.
        /// </summary>
        private static Vector2 _ScorePosition = new Vector2(245, 1);

        /// <summary>
        /// The <see cref="_Playground"/>.
        /// </summary>
        private Playground _Playground;

        /// <summary>
        /// List of balls.
        /// </summary>
        private List<Ball> _Balls = new List<Ball>();

        /// <summary>
        /// The <see cref="SpriteBatch"/> for drawing.
        /// </summary>
        private SpriteBatch _Batch;

        /// <summary>
        /// Initializes the <see cref="Gamestate"/>.
        /// </summary>
        public override void Initialize()
        {
            _Playground = new Playground(this);

            _Statsfont = Manager.Game.Content.LoadBitmapFont("Font");

            _Batch = Manager.Game.Batch;

            SpawnBall();
            SpawnPaddles();
            SpawnBlocks();
        }

        /// <summary>
        /// Destroys the <see cref="Gamestate"/>.
        /// </summary>
        public override void Destroy()
        {
            _Playground.Destroy();
            _Playground = null;
        }

        /// <summary>
        /// Updates the <see cref="Gamestate"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public override void Update(GameTime gameTime)
        {
            _Playground.Update(gameTime);

            UpdateBalls();
        }

        /// <summary>
        /// Draws the <see cref="Gamestate"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        /// <returns><c>True</c>, if the next gamestate may draw too.</returns>
        public override bool Draw(GameTime gameTime)
        {
            _Batch.Begin(SpriteSortMode.Texture,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone);

            _Playground.Draw(gameTime);

            // Draw status
            _Batch.DrawString(_Statsfont,
                Score.ToString().PadLeft(6, '0') + " Score",
                _ScorePosition,
                Color.White);

            _Batch.End();
            return false;
        }

        /// <summary>
        /// Spawns a new <see cref="Ball"/>.
        /// </summary>
        public void SpawnBall()
        {
            // Spawn ball in the middle of the screen
            Ball ball = new Ball();
            _Playground.Add(ball);
            ball.PhysicsBody.SetTransform(
                new Vector2(160f, 210f) * BreakoutPartyGame.MeterPerPixel,
                0f);

            Vector2 velocity =  new Vector2(
                (float)BreakoutPartyGame.Random.NextDouble() - 0.5f,
                -0.8f);
            velocity.Normalize();
            ball.PhysicsBody.LinearVelocity = velocity * 2.5f;

            _Balls.Add(ball);
        }

        /// <summary>
        /// Spawns the paddles.
        /// </summary>
        private void SpawnPaddles()
        {
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                Paddle paddle = new Paddle();
                _Playground.Add(paddle);
                paddle.Player = player;
                paddle.PaddleSpeed = 3f;

                if (player > PlayerIndex.One)
                    paddle.IsComputer = true;

                switch (player)
                {
                    case PlayerIndex.One:
                        paddle.PhysicsBody.Position = new Vector2(
                            160 * BreakoutPartyGame.MeterPerPixel,
                            230 * BreakoutPartyGame.MeterPerPixel);
                        break;
                    case PlayerIndex.Two:
                        paddle.PhysicsBody.Position = new Vector2(
                            160 * BreakoutPartyGame.MeterPerPixel,
                            10 * BreakoutPartyGame.MeterPerPixel);
                        paddle.PhysicsBody.Rotation = MathHelper.ToRadians(180f);
                        break;
                    case PlayerIndex.Three:
                        paddle.PhysicsBody.Position = new Vector2(
                            10 * BreakoutPartyGame.MeterPerPixel,
                            120 * BreakoutPartyGame.MeterPerPixel);
                        paddle.PhysicsBody.Rotation = MathHelper.ToRadians(90f);
                        break;
                    case PlayerIndex.Four:
                        paddle.PhysicsBody.Position = new Vector2(
                            310 * BreakoutPartyGame.MeterPerPixel,
                            120 * BreakoutPartyGame.MeterPerPixel);
                        paddle.PhysicsBody.Rotation = MathHelper.ToRadians(270f);
                        break;
                }
            }
        }

        /// <summary>
        /// Spawns the blocks.
        /// </summary>
        private void SpawnBlocks()
        {
            for(int x = 0; x < 6; x++)
            {
                for(int y = 0; y < 5; y++)
                {
                    Block block = new Block();
                    _Playground.Add(block);
                    block.PhysicsBody.Position = new Vector2(
                        (80 + x * 32f) * BreakoutPartyGame.MeterPerPixel,
                        (87 + y * 12f) * BreakoutPartyGame.MeterPerPixel);
                    block.Tint = new Color(
                        BreakoutPartyGame.Random.Next(150, 255),
                        BreakoutPartyGame.Random.Next(150, 255),
                        BreakoutPartyGame.Random.Next(150, 255));
                }
            }
        }

        /// <summary>
        /// Checks, if balls are inside the playground and spawns
        /// new ones if necessary.
        /// </summary>
        private void UpdateBalls()
        {
            for (int i = _Balls.Count - 1; i >= 0; i--)
            {
                var ball = _Balls[i];
                var physicsBody = ball.PhysicsBody;
                // Make ball stay inside bounds
                if (physicsBody.Position.X < 0f 
                    || physicsBody.Position.X > 320 * BreakoutPartyGame.MeterPerPixel
                    || physicsBody.Position.Y < 0f
                    || physicsBody.Position.Y > 240 * BreakoutPartyGame.MeterPerPixel)
                {
                    _Playground.Remove(ball);
                    _Balls.Remove(ball);
                    SpawnBall();
                }
            }
        }

    }
}
