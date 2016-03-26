using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Entities
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    abstract class Entity
    {
        /// <summary>
        /// The physical body.
        /// </summary>
        public FarseerPhysics.Dynamics.Body PhysicsBody;

        /// <summary>
        /// The <see cref="Playground"/> the <see cref="Entity"/> belongs to.
        /// </summary>
        public Playground Playground;

        /// <summary>
        /// Initializes the <see cref="Entity"/>. Is called once the entity
        /// has been added to a <see cref="Playground"/> and after the 
        /// <see cref="Entity.Playground"/> field has been set.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Destroys the <see cref="Entity"/>.
        /// </summary>
        public abstract void Destroy();

        /// <summary>
        /// Updates the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Makes the <see cref="Entity"/> bouncy.
        /// </summary>
        protected void MakePhysicsBodyBouncy()
        {
            PhysicsBody.Friction = 0f;
            PhysicsBody.Restitution = 1f;
            PhysicsBody.AngularDamping = 0f;
            PhysicsBody.LinearDamping = 0f;
            foreach(var fix in PhysicsBody.FixtureList)
            {
                fix.Friction = 0f;
                fix.Restitution = 1f;
            }
        }
    }
}
