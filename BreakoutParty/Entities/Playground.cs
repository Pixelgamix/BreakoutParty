using BreakoutParty.Gamestates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Entities
{
    /// <summary>
    /// Playground that manages entities.
    /// </summary>
    sealed class Playground
    {
        /// <summary>
        /// The <see cref="Gamestate"/> the <see cref="Playground"/> belongs
        /// to.
        /// </summary>
        public Gamestate State;

        /// <summary>
        /// The physics <see cref="FarseerPhysics.Dynamics.World"/>.
        /// </summary>
        public FarseerPhysics.Dynamics.World World;

        /// <summary>
        /// List of entities.
        /// </summary>
        private List<Entity> _Entities = new List<Entity>();

        /// <summary>
        /// Creates a new <see cref="Playground"/> instance.
        /// </summary>
        /// <param name="state">The <see cref="Gamestate"/> the instance is tied to.</param>
        public Playground(Gamestate state)
        {
            State = state;
            World = new FarseerPhysics.Dynamics.World(new Vector2(0f, 0.9f));
        }

        /// <summary>
        /// Returns all entities of the specified <see cref="Type"/>,
        /// </summary>
        /// <typeparam name="T">The entity type to retrieve.</typeparam>
        /// <returns>All entities of the specified type inside the <see cref="Playground"/>.</returns>
        public IEnumerable<T> GetEntities<T>() where T : Entity
        {
            for(int i = _Entities.Count - 1; i >= 0; i--)
            {
                T entity = _Entities[i] as T;
                if (entity != null)
                    yield return entity;
            }
        }

        /// <summary>
        /// Destroys the <see cref="Playground"/> destroying all
        /// entities.
        /// </summary>
        public void Destroy()
        {
            // Loop until there are no entities left. Is necessary, since
            // entities may spawn other entities as a result of their
            // destruction (for example: explosion entities).
            while (_Entities.Count > 0)
                Remove(_Entities[0]);
            World.Clear();
        }

        /// <summary>
        /// Adds the specified <see cref="Entity"/> to the <see cref="Playground"/>.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(Entity entity)
        {
            entity.Playground = this;
            entity.Initialize();
            _Entities.Add(entity);
        }

        /// <summary>
        /// Removes the specified <see cref="Entity"/> from the <see cref="Playground"/>
        /// and calls its <see cref="Entity.Destroy"/> method.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(Entity entity)
        {
            _Entities.Remove(entity);
            entity.Destroy();
        }

        /// <summary>
        /// Updates the world and all entities.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public void Update(GameTime gameTime)
        {
            // Minimum of 30 updates per second
            World.Step(MathHelper.Min(
                (float)gameTime.ElapsedGameTime.TotalSeconds,
                1f / 30f));

            int count = _Entities.Count;
            for (int i = count - 1; i >= 0; i--)
                _Entities[i].Update(gameTime);
        }

        /// <summary>
        /// Draws the entities.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public void Draw(GameTime gameTime)
        {
            int count = _Entities.Count;
            for (int i = count - 1; i >= 0; i--)
                _Entities[i].Draw(gameTime);
        }
    }
}
