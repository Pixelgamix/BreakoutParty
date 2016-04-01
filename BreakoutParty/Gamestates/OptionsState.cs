using BreakoutParty.Font;
using BreakoutParty.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Gamestates
{
    /// <summary>
    /// Options state.
    /// </summary>
    sealed class OptionsState : Gamestate
    {
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
        /// Currently selected option
        /// </summary>
        private int _SelectedOption;

        /// <summary>
        /// Initializes the <see cref="Gamestate"/>.
        /// </summary>
        public override void Initialize()
        {
            _TextFont = Manager.Game.Content.LoadBitmapFont("Font");
            _TitleFont = Manager.Game.Content.LoadBitmapFont("TitleFont");
            _Batch = Manager.Game.Batch;
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
            if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Abort))
            {
                Manager.Game.AudioManager.Play(SoundEffects.MenuBack);
                Manager.Remove(this);
                Manager.Add(new MainMenuGamestate());
            }
            else if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Up))
            {
                if (_SelectedOption > 0)
                    _SelectedOption--;
                else
                    _SelectedOption = 1;
                Manager.Game.AudioManager.Play(SoundEffects.MenuSelect);
            }
            else if(InputManager.IsActionPressed(PlayerIndex.One, InputActions.Down))
            {
                if (_SelectedOption < 1)
                    _SelectedOption++;
                else
                    _SelectedOption = 0;
                Manager.Game.AudioManager.Play(SoundEffects.MenuSelect);
            }
            else if(InputManager.IsActionPressed(PlayerIndex.One, InputActions.Left))
            {
                if(_SelectedOption == 0 && SoundEffect.MasterVolume > 0f)
                {
                    SoundEffect.MasterVolume = MathHelper.Max(0f, SoundEffect.MasterVolume - 0.05f);
                    Manager.Game.Data.SoundVolume = SoundEffect.MasterVolume;
                    Manager.Game.AudioManager.Play(SoundEffects.MenuValidate);
                }
                else if(_SelectedOption == 1 && MediaPlayer.Volume > 0f)
                {
                    MediaPlayer.Volume = MathHelper.Max(0f, MediaPlayer.Volume - 0.05f);
                    Manager.Game.Data.MusicVolume = MediaPlayer.Volume;
                }
            }
            else if(InputManager.IsActionPressed(PlayerIndex.One, InputActions.Right))
            {
                if (_SelectedOption == 0 && SoundEffect.MasterVolume < 1f)
                {
                    SoundEffect.MasterVolume = MathHelper.Min(1f, SoundEffect.MasterVolume + 0.05f);
                    Manager.Game.Data.SoundVolume = SoundEffect.MasterVolume;
                    Manager.Game.AudioManager.Play(SoundEffects.MenuValidate);
                }
                else if (_SelectedOption == 1 && MediaPlayer.Volume < 1f)
                {
                    MediaPlayer.Volume = MathHelper.Min(1f, MediaPlayer.Volume + 0.05f);
                    Manager.Game.Data.MusicVolume = MediaPlayer.Volume;
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

            // Title
            _Batch.DrawString(
                _TitleFont,
                "Options",
                new Vector2(130, 70),
                Color.White);

            float f = 0.7f + 0.3f * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 10);
            Color selectedColor = new Color(f, f, f);

            _Batch.DrawString(
                _TextFont,
                "Sound Volume: " + SoundEffect.MasterVolume.ToString("P"),
                new Vector2(110, 100),
                _SelectedOption == 0 ? selectedColor : Color.White);

            _Batch.DrawString(
                _TextFont,
                "Music Volume: " + MediaPlayer.Volume.ToString("P"),
                new Vector2(110, 120),
                _SelectedOption == 1 ? selectedColor : Color.White);

            _Batch.End();

            return false;
        }
    }
}
