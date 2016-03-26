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
        /// The block's tint.
        /// </summary>
        public Color Tint = Color.White;

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
        /// Initializes the <see cref="Entity"/>. Is called once the entity
        /// has been added to a <see cref="Playground"/> and after the 
        /// <see cref="Entity.Playground"/> field has been set.
        /// </summary>
        public override void Initialize()
        {
            BreakoutPartyGame game = Playground.State.Manager.Game;

            _Batch = game.Batch;

            _BlockTexture = game.Content.Load<Texture2D>("Block");

            _Origin = new Vector2(_BlockTexture.Width * 0.5f, _BlockTexture.Height * 0.5f);

            PhysicsBody = FarseerPhysics.Factories.BodyFactory.CreateRectangle(
                Playground.World,
                _BlockTexture.Width * BreakoutPartyGame.MeterPerPixel,
                _BlockTexture.Height * BreakoutPartyGame.MeterPerPixel,
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

            _Batch.Draw(
                _BlockTexture,
                PhysicsBody.Position * BreakoutPartyGame.PixelsPerMeter,
                null,
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
            PhysicsBody.CollisionCategories = CollisionGroups.None;
            PhysicsBody.IgnoreGravity = false;
            return true;
        }
    }
}
