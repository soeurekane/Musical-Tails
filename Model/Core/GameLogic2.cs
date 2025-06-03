using System;
using System.Diagnostics;

namespace Model.Core
{
    //часть 2
    public partial class GameLogic
    {
        public void EndGame()
        {
            _isGameOver = true;
            GameEnded?.Invoke();
        }

        public void RegisterMissedButton(IMusicalButton button)
        {
            Debug.WriteLine($"[MISS] {button.GetButtonType()} button missed");

            if (button is MusicalButtonBase { ShouldPenalizeOnMiss: true })
            {
                _missedButtons++;
                RemoveButtonFromArray(button);

                if (_missedButtons >= 3)
                {
                    _isGameOver = true;
                    Debug.WriteLine("[GAME OVER] Too many misses");
                    EndGame();
                }
            }
        }
    }
}