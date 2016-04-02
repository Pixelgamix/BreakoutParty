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
    /// "How To Play" information screen.
    /// </summary>
    sealed class HowToPlayState : Gamestate
    {
        /// <summary>
        /// <see cref="SpriteBatch"/> for drawing.
        /// </summary>
        private SpriteBatch _Batch;

        /// <summary>
        /// Texture showing how to play.
        /// </summary>
        private Texture2D _HowToPlayTexture;

        /// <summary>
        /// Initializes the <see cref="Gamestate"/>.
        /// </summary>
        public override void Initialize()
        {
            _Batch = Manager.Game.Batch;
            _HowToPlayTexture = Manager.Game.Content.Load<Texture2D>("HowToPlay");
            Manager.Game.AudioManager.Play(MusicTracks.TitleMusic);
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
            if (InputManager.IsActionPressed(PlayerIndex.One, InputActions.Ok)
                || InputManager.IsActionPressed(PlayerIndex.One, InputActions.Abort))
            {
                Manager.Game.AudioManager.Play(SoundEffects.MenuValidate);
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

            _Batch.Draw(_HowToPlayTexture, Vector2.Zero, Color.White);

            _Batch.End();

            return false;
        }
    }
}
