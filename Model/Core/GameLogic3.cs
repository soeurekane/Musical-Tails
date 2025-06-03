using Model.Core;

namespace Model.Core
{
    public partial class GameLogic
    {
        public void ProcessTrapButton(TrapButton trapButton)
        {
            AddScore(trapButton.ScoreValue);
            RemoveButtonFromArray(trapButton);

            if (trapButton.ScoreValue < 0)
                EndGame();
        }

        public void AddActiveButton(IMusicalButton button)
        {
            Array.Resize(ref _activeButtons, _activeButtons.Length + 1);
            _activeButtons[_activeButtons.Length - 1] = button;
        }
    }
}