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
    /// Destroyable block.
    /// </summary>
    sealed class Block : Entity
    {
        /// <summary>
        /// Width in pixels.
        /// </summary>
        public const int Width = 32;

        /// <summary>
        /// Height in pixels.
        /// </summary>
        public const int Height = 12;

        /// <summary>
        /// The block's tint.
        /// </summary>
        public Color Tint = Color.White;

        /// <summary>
        /// Block maximum health.
        /// </summary>
        public int MaxHealth;

        /// <summary>
        /// Block current health.
        /// </summary>
        public int Health;

        /// <summary>
        /// Colors to randomly choose the tint from.
        /// </summary>
        private static Color[] Colors = {
            Color.Orange,
            Color.LawnGreen,
            Color.LightBlue,
            Color.Yellow,
            Color.MediumPurple,
            Color.Gold,
            Color.Pink,
            Color.White
        };

        /// <summary>
        /// Block texture.
        /// </summary>
        private Texture2D _BlockTexture;

        /// <summary>
        /// Ball origin.
        /// </summary>
        private Vector2 _Origin;

        /// <summary>
        /// The <see cref="SpriteBatch"/> to draw with.
        /// </summary>
        private SpriteBatch _Batch;

        /// <summary>
        /// Creates a new <see cref="Block"/>.
        /// </summary>
        /// <param name="health">The health the block starts with.</param>
        public Block(int health)
        {
            MaxHealth = health;
            Health = health;

            Tint = Colors[BreakoutPartyGame.Random.Next(Colors.Length)];
        }

        /// <summary>
        /// Initializes the <see cref="Entity"/>. Is called once the entity
        /// has been added to a <see cref="Playground"/> and after the 
        /// <see cref="Entity.Playground"/> field has been set.
        /// </summary>
        public override void Initialize()
        {
            BreakoutPartyGame game = Playground.State.Manager.Game;

            _Batch = game.Batch;

            _BlockTexture = game.Content.Load<Texture2D>("Block");

            _Origin = new Vector2(Width * 0.5f, Height * 0.5f);

            PhysicsBody = FarseerPhysics.Factories.BodyFactory.CreateRectangle(
                Playground.World,
                Width * BreakoutPartyGame.MeterPerPixel,
                Height * BreakoutPartyGame.MeterPerPixel,
                1f,
                Vector2.Zero,
                0f,
                FarseerPhysics.Dynamics.BodyType.Dynamic,
                this);
            PhysicsBody.IgnoreGravity = true;
            PhysicsBody.CollisionCategories = CollisionGroups.Block;
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
            _BlockTexture = null;

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
            if (PhysicsBody.Position.X > 320 * BreakoutPartyGame.MeterPerPixel
                || PhysicsBody.Position.X < 0
                || PhysicsBody.Position.Y > 240 * BreakoutPartyGame.MeterPerPixel
                || PhysicsBody.Position.Y < 0)
                Playground.Remove(this);

            // Block cannot be moved as long as it has health left
            if (Health > 0)
            {
                PhysicsBody.LinearVelocity = Vector2.Zero;
                PhysicsBody.AngularVelocity = 0;
            }
        }

        /// <summary>
        /// Draws the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public override void Draw(GameTime gameTime)
        {
            Color color = Color.Lerp(Tint,
                Color.Transparent,
                PhysicsBody.LinearVelocity.LengthSquared() * 0.1f);

            Rectangle sourceRect = new Rectangle(
                0,
                Height * (int)(3f - Health / (float)MaxHealth * 3f),
                Width,
                Height);

            _Batch.Draw(
                _BlockTexture,
                PhysicsBody.Position * BreakoutPartyGame.PixelsPerMeter,
                sourceRect,
                color,
                PhysicsBody.Rotation,
                _Origin,
                1f, // Scale
                SpriteEffects.None,
                0f); // Depth
        }

        /// <summary>
        /// Handles collisions.
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        private bool PhysicsBody_OnCollision(FarseerPhysics.Dynamics.Fixture fixtureA,
            FarseerPhysics.Dynamics.Fixture fixtureB,
            FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (Health > 0)
            {
                Health--;
            }
            if (Health == 0)
            {
                var state = Playground.State as Gamestates.BreakoutState;
                state.Score++;

                // Random chance of spawning a ball
                if (BreakoutPartyGame.Random.NextDouble() > 0.9)
                    state.SpawnBall();

                PhysicsBody.CollisionCategories = CollisionGroups.None;
                PhysicsBody.IgnoreGravity = false;
            }

            return true;
        }
    }
}
