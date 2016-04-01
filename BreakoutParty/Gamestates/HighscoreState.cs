using BreakoutParty.Data;
using BreakoutParty.Font;
using BreakoutParty.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Gamestates
{
    /// <summary>
    /// <see cref="Gamestate"/> that displays highscores and allows
    /// the player to enter a new highscore.
    /// </summary>
    sealed class HighscoreState : Gamestate
    {
        /// <summary>
        /// Current players level.
        /// </summary>
        public int Level;

        /// <summary>
        /// Current players score.
        /// </summary>
        public int Score;

        /// <summary>
        /// Location of new highscore.
        /// </summary>
        private int _NewHighscore = -1;

        /// <summary>
        /// The current letter.
        /// </summary>
        private int _CurrentLetter = 0;

        /// <summary>
        /// <see cref="SpriteBatch"/> for drawing.
        /// </summary>
        private SpriteBatch _Batch;

        /// <summary>
        /// <see cref="BitmapFont"/> for drawing the credits.
        /// </summary>
        private BitmapFont _TextFont;

        /// <summary>
        /// <see cref="BitmapFont"/> for drawing the title texts.
        /// </summary>
        private BitmapFont _TitleFont;

        /// <summary>
        /// Creates a new <see cref="HighscoreState"/>.
        /// </summary>
        /// <param name="level">The level the player reached.</param>
        /// <param name="score">The score the player reached.</param>
        public HighscoreState(int level, int score)
        {
            Level = level;
            Score = score;
        }

        /// <summary>
        /// Initializes the <see cref="Gamestate"/>.
        /// </summary>
        public override void Initialize()
        {
            _TextFont = Manager.Game.Content.LoadBitmapFont("Font");
            _TitleFont = Manager.Game.Content.LoadBitmapFont("TitleFont");
            _Batch = Manager.Game.Batch;

            // New highscore?
            for(int i = 0; i < Gamedata.MaxHighscoreCount; i++)
            {
                Highscore highscore = Manager.Game.Data.Highscores[i];
                if(Score > highscore.Score)
                {
                    highscore.Name = "AAA";
                    highscore.Score = Score;
                    highscore.Level = Level;
                    highscore.Date = DateTime.Now;
                    _NewHighscore = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Destroys the <see cref="Gamestate"/>.
        /// </summary>
        public override void Destroy()
        {

        }

        /// <summary>
        /// Updates the <see cref="Gamestate"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        public override void Update(GameTime gameTime)
        {
            // Letter input for name
            if(_NewHighscore >= 0)
            {
                if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Up))
                {
                    Manager.Game.AudioManager.Play(SoundEffects.MenuSelect);
                    var name = new StringBuilder(Manager.Game.Data.Highscores[_NewHighscore].Name);
                    if (name[_CurrentLetter] < 'Z')
                        name[_CurrentLetter]++;
                    else
                        name[_CurrentLetter] = 'A';
                    Manager.Game.Data.Highscores[_NewHighscore].Name = name.ToString();
                }
                else if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Down))
                {
                    Manager.Game.AudioManager.Play(SoundEffects.MenuSelect);
                    var name = new StringBuilder(Manager.Game.Data.Highscores[_NewHighscore].Name);
                    if (name[_CurrentLetter] > 'A')
                        name[_CurrentLetter]--;
                    else
                        name[_CurrentLetter] = 'Z';
                    Manager.Game.Data.Highscores[_NewHighscore].Name = name.ToString();
                }
                else if(InputManager.IsActionPressed(PlayerIndex.One, InputActions.Left))
                {
                    if(_CurrentLetter > 0)
                    {
                        Manager.Game.AudioManager.Play(SoundEffects.MenuSelect);
                        _CurrentLetter--;
                    }
                }
                else if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Right))
                {
                    if (_CurrentLetter < 2)
                    {
                        Manager.Game.AudioManager.Play(SoundEffects.MenuSelect);
                        _CurrentLetter++;
                    }
                }
            }

            if(InputManager.IsActionPressed(PlayerIndex.One, InputActions.Abort)
                || InputManager.IsActionPressed(PlayerIndex.One, InputActions.Ok))
            {
                // Abort name entering
                if (_NewHighscore >= 0)
                    _NewHighscore = -1;
                else
                {
                    // Return to main menu
                    Manager.Game.AudioManager.Play(SoundEffects.MenuBack);
                    Manager.Remove(this);
                    Manager.Add(new MainMenuGamestate());
                }
            }
        }

        /// <summary>
        /// Draws the <see cref="Gamestate"/>.
        /// </summary>
        /// <param name="gameTime">Timing information.</param>
        /// <returns><c>True</c>, if the next gamestate may draw too.</returns>
        public override bool Draw(GameTime gameTime)
        {
            _Batch.Begin(SpriteSortMode.Texture,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone);

            float f = 0.7f + 0.3f * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 10);
            Color selectedColor = new Color(f, f, f);

            // Draw title
            _Batch.DrawString(_TitleFont,
                "Highscores",
                new Vector2(
                    130f,
                    15f),
                Color.White);

            // Draw colum headers
            _Batch.DrawString(_TextFont,
                    "Name",
                    new Vector2(40, 40f),
                    Color.White);

            _Batch.DrawString(_TextFont,
                "Date",
                new Vector2(80, 40f),
                Color.White);

            _Batch.DrawString(_TextFont,
                "Level",
                new Vector2(162, 40f),
                Color.White);

            _Batch.DrawString(_TextFont,
                "Score",
                new Vector2(220, 40f),
                Color.White);

            // Draw highscore entries
            for (int i = 0; i < Gamedata.MaxHighscoreCount; i++)
            {
                Highscore highscore = Manager.Game.Data.Highscores[i];
                Color color = Color.White;

                // If we have a new highscore, then we only show
                // the new entry in bright color. All other entries
                // are darker.
                if (_NewHighscore >= 0 && i != _NewHighscore)
                    color = Color.Gray;

                if (_NewHighscore < 0 || _NewHighscore >= 0 && i != _NewHighscore)
                {
                    // Draw a normal entry
                    _Batch.DrawString(_TextFont,
                        highscore.Name,
                        new Vector2(40, i * 20f + 60f),
                        color);
                }
                else
                {
                    // Draw each character in the name text
                    // alone only highlighting the current one.
                    for(int c = 0; c < 3; c++)
                    {
                        _Batch.DrawString(_TextFont,
                        highscore.Name[c].ToString(),
                        new Vector2(40 + c * 8, i * 20f + 60f),
                        _CurrentLetter == c ? selectedColor : Color.White);
                    }
                }

                _Batch.DrawString(_TextFont,
                    highscore.Date.ToShortDateString(),
                    new Vector2(80, i * 20f + 60f),
                    color);

                _Batch.DrawString(_TextFont,
                    highscore.Level.ToString().PadLeft(2, '0'),
                    new Vector2(180, i * 20f + 60f),
                    color);

                _Batch.DrawString(_TextFont,
                    highscore.Score.ToString().PadLeft(6, '0'),
                    new Vector2(220, i * 20f + 60f),
                    color);
            }

            // New highscore message
            if(_NewHighscore >= 0)
            {
                _Batch.DrawString(
                    _TitleFont,
                    "New Highscore!",
                    new Vector2(100f, 210f),
                    (int)(gameTime.TotalGameTime.TotalSeconds) % 2 == 0 ? Color.White : Color.Yellow);
            }

            _Batch.End();

            return false;
        }
    }
}
