using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Sounds
{
    /// <summary>
    /// Manages sound effects and music.
    /// </summary>
    sealed class SoundManager
    {
        /// <summary>
        /// The <see cref="BreakoutPartyGame"/> the manager belongs to.
        /// </summary>
        private BreakoutPartyGame _Game;
        
        /// <summary>
        /// The current music.
        /// </summary>
        private MusicTracks _CurrentMusic = MusicTracks.None;

        /// <summary>
        /// Creates a new <see cref="SoundManager"/> instance.
        /// </summary>
        /// <param name="game">The <see cref="BreakoutPartyGame"/> the manager belongs to.</param>
        public SoundManager(BreakoutPartyGame game)
        {
            _Game = game;
            SoundEffect.MasterVolume = game.Data.SoundVolume;
            MediaPlayer.Volume = game.Data.MusicVolume;
        }

        /// <summary>
        /// Plays the specified sound once.
        /// </summary>
        /// <param name="effect">The sound to play.</param>
        public void Play(SoundEffects effect)
        {
            SoundEffect data = _Game.Content.Load<SoundEffect>(Enum.GetName(typeof(SoundEffects), effect));
            data.Play();
        }

        /// <summary>
        /// Plays the specified music in repeat mode. Playing "None"
        /// stops the music.
        /// </summary>
        /// <param name="music">The music to play.</param>
        public void Play(MusicTracks music)
        {
            if (music == _CurrentMusic)
                return;

            if (music == MusicTracks.None)
                MediaPlayer.Stop();
            else
            {
                Song data = _Game.Content.Load<Song>(Enum.GetName(typeof(MusicTracks), music));
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(data);
            }
            _CurrentMusic = music;
        }
    }
}
