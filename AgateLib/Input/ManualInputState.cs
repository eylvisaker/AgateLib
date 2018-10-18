using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.Input
{
    /// <summary>
    /// An object that implements IInputState that can be configured to simulate any input.
    /// </summary>
    public class ManualInputState : IInputState
    {
        Dictionary<PlayerIndex, GamePadState> gamePadState = new Dictionary<PlayerIndex, GamePadState>();

        public ManualInputState()
        {
            Reset();
        }

        public void Reset()
        {
            gamePadState[PlayerIndex.One] = new GamePadState();
            gamePadState[PlayerIndex.Two] = new GamePadState();
            gamePadState[PlayerIndex.Three] = new GamePadState();
            gamePadState[PlayerIndex.Four] = new GamePadState();

            KeyboardState = new KeyboardState();
        }

        public GameTime GameTime { get; set; }

        public KeyboardState KeyboardState { get; set; }

        public GamePadState GamePadStateOf(PlayerIndex playerIndex) => gamePadState[playerIndex];

        public Dictionary<PlayerIndex, GamePadState> GamePadState => gamePadState;
    }
}
