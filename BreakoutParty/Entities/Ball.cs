using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Entities
{
    /// <summary>
    /// A ball.
    /// </summary>
    sealed class Ball : Entity
    {
        /// <summary>
        /// Ball texture.
        /// </summary>
        private Texture2D _BallTexture;

        /// <summary>
        /// Ball origin.
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

            _BallTexture = game.Content.Load<Texture2D>("Ball");

            _Origin = new Vector2(_BallTexture.Width * 0.5f, _BallTexture.Height * 0.5f);

            PhysicsBody = FarseerPhysics.Factories.BodyFactory.CreateCircle(
                Playground.World,
                _BallTexture.Width * 0.5f * BreakoutPartyGame.MeterPerPixel,
                1f,
                Vector2.Zero,
                FarseerPhysics.Dynamics.BodyType.Dynamic,
                this);
            PhysicsBody.IgnoreGravity = true;
            PhysicsBody.CollisionCategories = CollisionGroups.Ball;
            PhysicsBody.CollidesWith = CollisionGroups.Ball
                | CollisionGroups.Block
                | CollisionGroups.Paddle;
            MakePhysicsBodyBouncy();
        }

        /// <summary>
        /// Destroys the <see cref="Entity"/>.
        /// </summary>
        public override void Destroy()
        {
            _Batch = null;
            _BallTexture = null;

            Playground.World.RemoveBody(PhysicsBody);
            PhysicsBody.UserData = null;
            PhysicsBody = null;
        }

        /// <summary>
        /// Updates the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public override void Update(GameTime gameTime)
        {
            // Minimum speed is 1.2 m/s
            Vector2 velocity = PhysicsBody.LinearVelocity;
            if (velocity.LengthSquared() < 1f)
            {
                velocity.Normalize();
                PhysicsBody.LinearVelocity = velocity * 1.2f;
            }
            else
            {
                velocity.Normalize();
                velocity *= 0.1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                PhysicsBody.ApplyForce(velocity);
            }
        }

        /// <summary>
        /// Draws the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public override void Draw(GameTime gameTime)
        {
            _Batch.Draw(
                _BallTexture,
                PhysicsBody.Position * BreakoutPartyGame.PixelsPerMeter,
                null,
                Color.White,
                PhysicsBody.Rotation,
                _Origin,
                1f, // Scale
                SpriteEffects.None,
                0f); // Depth
        }
    }
}
