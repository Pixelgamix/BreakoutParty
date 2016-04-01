using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Data
{
    /// <summary>
    /// Highscore data container.
    /// </summary>
    public sealed class Highscore
    {
        /// <summary>
        /// Three letter name.
        /// </summary>
        public string Name = "AAA";

        /// <summary>
        /// <see cref="DateTime"/> of highscore creation.
        /// </summary>
        public DateTime Date = DateTime.Now;

        /// <summary>
        /// Level.
        /// </summary>
        public int Level = 0;

        /// <summary>
        /// Score.
        /// </summary>
        public int Score = 0;
    }
}
