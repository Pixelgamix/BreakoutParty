using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BreakoutParty.Gamestates
{
    /// <summary>
    /// Manages <see cref="Gamestate"/>s.
    /// </summary>
    sealed class GamestateManager
    {
        /// <summary>
        /// Returns <c>true</c>, if there is no <see cref="Gamestate"/> being
        /// managed by this instance.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Gamestates.Count == 0;
            }
        }

        /// <summary>
        /// The <see cref="BreakoutPartyGame"/> the manager belongs to.
        /// </summary>
        public readonly BreakoutPartyGame Game;

        /// <summary>
        /// List of <see cref="Gamestate"/>s.
        /// </summary>
        private List<Gamestate> _Gamestates = new List<Gamestate>();

        /// <summary>
        /// Creates a new <see cref="GamestateManager"/> instance.
        /// </summary>
        /// <param name="game">The <see cref="BreakoutPartyGame"/>.</param>
        public GamestateManager(BreakoutPartyGame game)
        {
            Game = game;
        }

        /// <summary>
        /// Adds the specified <see cref="Gamestate"/> to the list of
        /// managed gamestates, sets the state's <see cref="Gamestate.Manager"/>
        /// value to <c>this</c> and calls the state's <see cref="Gamestate.Initialize"/>
        /// method.
        /// </summary>
        /// <param name="state">The gamestate to add.</param>
        public void Add(Gamestate state)
        {
            _Gamestates.Insert(0, state);
            state.Manager = this;
            state.Initialize();
        }

        /// <summary>
        /// Removes the specified <see cref="Gamestate"/> from the list
        /// of managed gamestates and calls the state's <see cref="Gamestate.Destroy"/>
        /// method.
        /// </summary>
        /// <param name="state"></param>
        public void Remove(Gamestate state)
        {
            _Gamestates.Remove(state);
            state.Destroy();
        }

        /// <summary>
        /// Updates the <see cref="GamestateManager"/> and the topmost
        /// <see cref="Gamestate"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public void Update(GameTime gameTime)
        {
            if(_Gamestates.Count > 0)
                _Gamestates[0].Update(gameTime);
        }

        /// <summary>
        /// Draws <see cref="Gamestate"/>s beginning with the topmost
        /// one. The return value of each method determines if lower level
        /// gamestates are allowed to draw, too.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public void Draw(GameTime gameTime)
        {
            int count = _Gamestates.Count;
            for (int i = 0; i < count; i++)
                if (!_Gamestates[i].Draw(gameTime))
                    return;
        }
    }
}
