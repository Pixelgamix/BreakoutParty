using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty
{
    /// <summary>
    /// Handles user input.
    /// </summary>
    static class InputManager
    {
        /// <summary>
        /// Previous input state.
        /// </summary>
        private static bool[,] _PreviousInput = new bool[4,6];

        /// <summary>
        /// Current input state.
        /// </summary>
        private static bool[,] _CurrentInput = new bool[4,6];

        /// <summary>
        /// Updates user inputs.
        /// </summary>
        public static void Update()
        {
            // Safe old input
            var tmp = _PreviousInput;
            _PreviousInput = _CurrentInput;
            _CurrentInput = tmp;

            // Reset current input
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 6; y++)
                    _CurrentInput[x, y] = false;

            // Scan input
            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                var gamepad = GamePad.GetState(player);
                int pi = (int)player;
                if(gamepad.IsConnected)
                {

                    _CurrentInput[pi, (int)InputActions.Abort] = gamepad.IsButtonDown(Buttons.B);
                    _CurrentInput[pi, (int)InputActions.Down] = gamepad.IsButtonDown(Buttons.LeftThumbstickDown)
                        || gamepad.IsButtonDown(Buttons.DPadDown)
                        || gamepad.IsButtonDown(Buttons.RightThumbstickDown);
                    _CurrentInput[pi, (int)InputActions.Left] = gamepad.IsButtonDown(Buttons.LeftThumbstickLeft)
                        || gamepad.IsButtonDown(Buttons.DPadLeft)
                        || gamepad.IsButtonDown(Buttons.RightThumbstickLeft);
                    _CurrentInput[pi, (int)InputActions.Ok] = gamepad.IsButtonDown(Buttons.A);
                    _CurrentInput[pi, (int)InputActions.Right] = gamepad.IsButtonDown(Buttons.LeftThumbstickRight)
                        || gamepad.IsButtonDown(Buttons.DPadRight)
                        || gamepad.IsButtonDown(Buttons.RightThumbstickRight);
                    _CurrentInput[pi, (int)InputActions.Up] = gamepad.IsButtonDown(Buttons.LeftThumbstickUp)
                        || gamepad.IsButtonDown(Buttons.DPadUp)
                        || gamepad.IsButtonDown(Buttons.RightThumbstickUp);
                }
            }

            // Keyboard fallback for player 1
            var keyboard = Keyboard.GetState();
            _CurrentInput[0, (int)InputActions.Abort] |= keyboard.IsKeyDown(Keys.Escape);
            _CurrentInput[0, (int)InputActions.Down] |= keyboard.IsKeyDown(Keys.Down);
            _CurrentInput[0, (int)InputActions.Left] |= keyboard.IsKeyDown(Keys.Left);
            _CurrentInput[0, (int)InputActions.Ok] |= keyboard.IsKeyDown(Keys.Enter);
            _CurrentInput[0, (int)InputActions.Right] |= keyboard.IsKeyDown(Keys.Right);
            _CurrentInput[0, (int)InputActions.Up] |= keyboard.IsKeyDown(Keys.Up);

            // Keyboard fallback for player 2
            _CurrentInput[1, (int)InputActions.Abort] |= keyboard.IsKeyDown(Keys.Tab);
            _CurrentInput[1, (int)InputActions.Down] |= keyboard.IsKeyDown(Keys.S);
            _CurrentInput[1, (int)InputActions.Left] |= keyboard.IsKeyDown(Keys.A);
            _CurrentInput[1, (int)InputActions.Ok] |= keyboard.IsKeyDown(Keys.LeftShift);
            _CurrentInput[1, (int)InputActions.Right] |= keyboard.IsKeyDown(Keys.D);
            _CurrentInput[1, (int)InputActions.Up] |= keyboard.IsKeyDown(Keys.W);
        }

        /// <summary>
        /// Checks, if the specified <see cref="InputActions"/> is active.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="action">The action.</param>
        public static bool IsActionActive(PlayerIndex player, InputActions action)
        {
            return _CurrentInput[(int)player, (int)action];
        }
    }
}
