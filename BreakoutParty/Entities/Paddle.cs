using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Entities
{
    /// <summary>
    /// A player paddle.
    /// </summary>
    sealed class Paddle : Entity
    {
        /// <summary>
        /// The player that controls the paddle.
        /// </summary>
        public PlayerIndex Player;

        /// <summary>
        /// <c>True</c>, if the paddle is controlled by the computer.
        /// </summary>
        public bool IsComputer;

        /// <summary>
        /// Paddle's speed in m/s.
        /// </summary>
        public float PaddleSpeed = 1.5f;

        /// <summary>
        /// Paddle texture.
        /// </summary>
        private Texture2D _PaddleTexture;

        /// <summary>
        /// Sound for hits against the paddle.
        /// </summary>
        private SoundEffect _PaddleHitSound;

        /// <summary>
        /// Paddle origin.
        /// </summary>
        private Vector2 _Origin;

        /// <summary>
        /// The <see cref="SpriteBatch"/> to draw with.
        /// </summary>
        private SpriteBatch _Batch;

        /// <summary>
        /// Initializes the <see cref="Entity"/>. Is called once the entity
        /// has been added to a <see cref="Playground"/> and after the 
        /// <see cref="Entity.Playground"/> field has been set.
        /// </summary>
        public override void Initialize()
        {
            BreakoutPartyGame game = Playground.State.Manager.Game;

            _Batch = game.Batch;

            _PaddleTexture = game.Content.Load<Texture2D>("Paddle");
            _PaddleHitSound = game.Content.Load<SoundEffect>("PaddleHit");

            _Origin = new Vector2(_PaddleTexture.Width * 0.5f, _PaddleTexture.Height * 0.5f);

            PhysicsBody = FarseerPhysics.Factories.BodyFactory.CreateRoundedRectangle(
                Playground.World,
                _PaddleTexture.Width * BreakoutPartyGame.MeterPerPixel,
                _PaddleTexture.Height * BreakoutPartyGame.MeterPerPixel,
                12 * BreakoutPartyGame.MeterPerPixel,
                3 * BreakoutPartyGame.MeterPerPixel,
                6,
                1f,
                Vector2.Zero,
                0f,
                FarseerPhysics.Dynamics.BodyType.Kinematic,
                this);
            PhysicsBody.IgnoreGravity = true;
            PhysicsBody.CollisionCategories = CollisionGroups.Paddle;
            PhysicsBody.CollidesWith = CollisionGroups.Ball;
            PhysicsBody.OnCollision += PhysicsBody_OnCollision;
            MakePhysicsBodyBouncy();
        }

        /// <summary>
        /// Destroys the <see cref="Entity"/>.
        /// </summary>
        public override void Destroy()
        {
            _Batch = null;
            _PaddleTexture = null;

            Playground.World.RemoveBody(PhysicsBody);
            PhysicsBody.OnCollision -= PhysicsBody_OnCollision;
            PhysicsBody.UserData = null;
            PhysicsBody = null;
        }

        /// <summary>
        /// Updates the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsComputer)
                HandleComputer(gameTime);
            else
                HandleUserInput();

            // Make sure that the paddle stays within bounds
            if (Player == PlayerIndex.One || Player == PlayerIndex.Two)
            {
                if (PhysicsBody.Position.X < 16 * BreakoutPartyGame.MeterPerPixel)
                    PhysicsBody.LinearVelocity = new Vector2(PaddleSpeed, 0);
                else if (PhysicsBody.Position.X > (320 - 16) * BreakoutPartyGame.MeterPerPixel)
                    PhysicsBody.LinearVelocity = new Vector2(-PaddleSpeed, 0);
            }
            else
            {
                if (PhysicsBody.Position.Y < 32 * BreakoutPartyGame.MeterPerPixel)
                    PhysicsBody.LinearVelocity = new Vector2(0, PaddleSpeed);
                else if (PhysicsBody.Position.Y > (240 - 16) * BreakoutPartyGame.MeterPerPixel)
                    PhysicsBody.LinearVelocity = new Vector2(0, -PaddleSpeed);
            }
        }

        /// <summary>
        /// Draws the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public override void Draw(GameTime gameTime)
        {
            _Batch.Draw(
                _PaddleTexture,
                PhysicsBody.Position * BreakoutPartyGame.PixelsPerMeter,
                null,
                Color.White,
                PhysicsBody.Rotation,
                _Origin,
                1f, // Scale
                SpriteEffects.None,
                0f); // Depth
        }

        /// <summary>
        /// Handles user input.
        /// </summary>
        private void HandleUserInput()
        {
            float xd = InputManager.IsActionActive(Player, InputActions.Down) ? PaddleSpeed : 0;
            xd += InputManager.IsActionActive(Player, InputActions.Left) ? -PaddleSpeed : 0;
            xd += InputManager.IsActionActive(Player, InputActions.Right) ? PaddleSpeed : 0;
            xd += InputManager.IsActionActive(Player, InputActions.Up) ? -PaddleSpeed : 0;
            Vector2 velocity = new Vector2();
            if (Player == PlayerIndex.One || Player == PlayerIndex.Two)
                velocity.X = MathHelper.Clamp(xd, -PaddleSpeed, PaddleSpeed);
            else
                velocity.Y = MathHelper.Clamp(xd, -PaddleSpeed, PaddleSpeed);
            PhysicsBody.LinearVelocity = velocity;
        }

        /// <summary>
        /// Handles computer input.
        /// </summary>
        private void HandleComputer(GameTime gameTime)
        {
            Vector2 closestBall = Vector2.Zero;
            float closestDistance = float.MaxValue;
            foreach (Ball ball in Playground.GetEntities<Ball>())
            {
                Vector2 otherpos = ball.PhysicsBody.Position + ball.PhysicsBody.LinearVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                float distance = Vector2.DistanceSquared(PhysicsBody.Position, otherpos);
                if (closestBall == null
                    || distance < closestDistance)
                {
                    closestBall = otherpos;
                    closestDistance = distance;
                }
            }

            if (closestDistance != float.MaxValue)
            {
                // Move paddle towards ball
                Vector2 velocity = Vector2.Zero;
                if (Player == PlayerIndex.One || Player == PlayerIndex.Two)
                {
                    float xd = closestBall.X - PhysicsBody.Position.X;
                    if (Math.Abs(xd) > 8f * BreakoutPartyGame.MeterPerPixel)
                        velocity.X = xd < 0 ? -PaddleSpeed : PaddleSpeed;
                }
                else
                {
                    float yd = closestBall.Y - PhysicsBody.Position.Y;
                    if (Math.Abs(yd) > 8f * BreakoutPartyGame.MeterPerPixel)
                        velocity.Y = yd < 0 ? -PaddleSpeed : PaddleSpeed;
                }
                PhysicsBody.LinearVelocity = velocity;
            }
        }

        /// <summary>
        /// Handles collisions with this <see cref="Paddle"/>.
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        private bool PhysicsBody_OnCollision(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            _PaddleHitSound.Play();
            return true;
        }
    }
}
