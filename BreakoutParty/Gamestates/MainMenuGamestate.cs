using BreakoutParty.Font;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BreakoutParty.Gamestates
{
    /// <summary>
    /// Main menu
    /// </summary>
    sealed class MainMenuGamestate : Gamestate
    {
        /// <summary>
        /// BreakoutParty logo.
        /// </summary>
        private Texture2D _LogoTexture;

        /// <summary>
        /// <see cref="BitmapFont"/> for drawing the menu entries.
        /// </summary>
        private BitmapFont _MenuFont;

        /// <summary>
        /// <see cref="SpriteBatch"/> for drawing.
        /// </summary>
        private SpriteBatch _Batch;

        /// <summary>
        /// Menu entries.
        /// </summary>
        private static readonly string[] _MenuEntries = {
            "Start Local Game",
            "Watch Credits",
            "End Game"
        };

        /// <summary>
        /// Currently selected menu entry.
        /// </summary>
        private int _SelectedMenuEntry = 0;

        /// <summary>
        /// Game version
        /// </summary>
        private string _Version;

        /// <summary>
        /// Initializes the <see cref="Gamestate"/>.
        /// </summary>
        public override void Initialize()
        {
            ContentManager content = Manager.Game.Content;
            _LogoTexture = content.Load<Texture2D>("Title");
            _MenuFont = content.LoadBitmapFont("Font");
            _Batch = Manager.Game.Batch;

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            _Version = "V" + version.Major + "." + version.Minor;
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
            if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Up) && _SelectedMenuEntry > 0)
                _SelectedMenuEntry--;

            else if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Down) && _SelectedMenuEntry < _MenuEntries.Length - 1)
                _SelectedMenuEntry++;

            else if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Ok))
            {
                switch(_SelectedMenuEntry)
                {
                    case 0: // Start Local Game
                        Manager.Remove(this);
                        Manager.Add(new BreakoutState());
                        break;

                    case 1: // Credits
                        // TODO: Go to credits
                        break;

                    case 2: // Exit
                        Manager.Remove(this);
                        break;
                }
            }
            else if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Abort))
                Manager.Remove(this);
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

            _Batch.Draw(_LogoTexture,
                new Vector2(
                    160 + 20f * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 0.5),
                    60 +  15f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 0.5f)),
                null,
                Color.White,
                0.5f * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds),
                new Vector2(80, 60),
                1f + 0.2f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds),
                SpriteEffects.None,
                0f);

            for(int i = 0; i < _MenuEntries.Length; i++)
            {
                Color color = Color.White;
                if(_SelectedMenuEntry == i)
                {
                    float f = 0.7f + 0.3f * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 10);
                    color = new Color(f, f, f);
                }
                _Batch.DrawString(
                    _MenuFont,
                    _MenuEntries[i],
                    new Vector2(
                        160 - _MenuFont.MeasureFont(_MenuEntries[i]).X * 0.5f,
                        160 + i * 16),
                    color);
            }

            _Batch.DrawString(
                _MenuFont,
                _Version,
                new Vector2(5, 225),
                Color.Gray);

            _Batch.End();

            return false;
        }
    }
}
