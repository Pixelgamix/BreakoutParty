using BreakoutParty.Font;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Gamestates
{
    /// <summary>
    /// Credits screen.
    /// </summary>
    sealed class CreditsState : Gamestate
    {
        /// <summary>
        /// Credits text.
        /// </summary>
        private static string[] _Credits;

        /// <summary>
        /// Sound for going back in a menu.
        /// </summary>
        private SoundEffect _MenuBackSound;

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
        /// Credits position.
        /// </summary>
        private float _Position = 0f;

        /// <summary>
        /// Initializes the <see cref="Gamestate"/>.
        /// </summary>
        public override void Initialize()
        {
            _TextFont = Manager.Game.Content.LoadBitmapFont("Font");
            _TitleFont = Manager.Game.Content.LoadBitmapFont("TitleFont");
            _Batch = Manager.Game.Batch;
            _MenuBackSound = Manager.Game.Content.Load<SoundEffect>("MenuBack");

            if(_Credits == null)
            {
                _Credits = System.IO.File.ReadAllLines(
                    System.IO.Path.Combine(Manager.Game.Content.RootDirectory, "Credits.txt"));
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
            _Position += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if((_Credits.Length - _Position) * 24f + 240f < 0f
                || InputManager.IsActionPressed(PlayerIndex.One, InputActions.Ok)
                || InputManager.IsActionPressed(PlayerIndex.One, InputActions.Abort))
            {
                _MenuBackSound.Play();
                Manager.Remove(this);
                Manager.Add(new MainMenuGamestate());
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

            for(int i = 0; i < _Credits.Length; i++)
            {
                float y = (i - _Position) * 24f + 240f;
                string text = _Credits[i];
                if (y > -24f && y < 240f && text.Length > 0)
                {
                    float xd = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds + i * 0.2) * 24f;
                    BitmapFont font = text[0] == '[' ? _TitleFont : _TextFont;

                    Vector2 pos = new Vector2(
                        160f - font.MeasureFont(text).X * 0.5f + xd,
                        y);

                    _Batch.DrawString(
                        font,
                        text,
                        pos,
                        Color.White);
                }
            }

            _Batch.End();

            return false;
        }
    }
}
