using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Gamestates
{
    /// <summary>
    /// Abstract base class for <see cref="Gamestate"/>s.
    /// </summary>
    abstract class Gamestate
    {
        /// <summary>
        /// The <see cref="GamestateManager"/> the <see cref="Gamestate"/>
        /// is managed with.
        /// </summary>
        public GamestateManager Manager;

        /// <summary>
        /// Initializes the <see cref="Gamestate"/>.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Destroys the <see cref="Gamestate"/>.
        /// </summary>
        public abstract void Destroy();

        /// <summary>
        /// Updates the <see cref="Gamestate"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the <see cref="Gamestate"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        /// <returns><c>True</c>, if the next gamestate may draw too.</returns>
        public abstract bool Draw(GameTime gameTime);
    }
}
