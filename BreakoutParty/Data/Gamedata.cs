using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BreakoutParty.Data
{
    /// <summary>
    /// Game data container.
    /// </summary>
    public sealed class Gamedata
    {
        /// <summary>
        /// Maximum number of highscore entries.
        /// </summary>
        public const int MaxHighscoreCount = 6;

        /// <summary>
        /// Name of the savefile.
        /// </summary>
        public const string Savefile = "Gamedata.xml";

        /// <summary>
        /// Sound effect volume.
        /// </summary>
        public float SoundVolume = 1f;

        /// <summary>
        /// Music volume.
        /// </summary>
        public float MusicVolume = 1f;

        /// <summary>
        /// List of highscores.
        /// </summary>
        public List<Highscore> Highscores = new List<Highscore>();

        /// <summary>
        /// Saves the game's data.
        /// </summary>
        public void Save()
        {
            Directory.CreateDirectory(Utils.SaveDirectory);
            string file = Path.Combine(Utils.SaveDirectory, Savefile);
            XmlSerializer serializer = new XmlSerializer(typeof(Gamedata));
            using (var stream = File.Create(file))
                serializer.Serialize(stream, this);
        }

        /// <summary>
        /// Loads the game's data.
        /// </summary>
        /// <returns>The loaded <see cref="Gamedata"/>.</returns>
        public static Gamedata Load()
        {
            string file = Path.Combine(Utils.SaveDirectory, Savefile);
            if (!Directory.Exists(Utils.SaveDirectory)
                || !File.Exists(file))
                return CreateDefaultData();
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Gamedata));
                using (var stream = File.OpenRead(file))
                    return serializer.Deserialize(stream) as Gamedata;
            }
        }

        /// <summary>
        /// Creates default game data.
        /// </summary>
        private static Gamedata CreateDefaultData()
        {
            Gamedata data = new Gamedata();
            for (int i = MaxHighscoreCount - 1; i >= 0; i--)
            {
                Highscore highscore = new Highscore();
                highscore.Level = i * 2 + 1;
                highscore.Score = highscore.Level * 30;
                data.Highscores.Add(highscore);
            }
            return data;
        }
    }
}
