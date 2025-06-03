using System;
using System.Diagnostics;

namespace Model.Core
{
    //часть 1 
    public partial class GameLogic : IGameController
    {
        protected IMusicalButton[] _activeButtons = Array.Empty<IMusicalButton>();
        protected int _missedButtons = 0;
        protected int _score = 0;
        protected bool _isGameOver = false;
        protected readonly Random _random = new();

        public int Difficulty { get; }
        public int CurrentScore => _score;
        public int Score => _score;
        public int MissedButtons => _missedButtons;
        public bool IsGameOver => _isGameOver;
        public IButtonGenerator ButtonGenerator { get; } = new ButtonGenerator();

        public event Action<int>? ScoreUpdated;
        public event Action? GameEnded;

        public int LaneCount { get; }

        public GameLogic(int laneCount, int difficulty)
        {
            LaneCount = laneCount;
            Difficulty = difficulty;
        }

        public void AddScore(int points)
        {
            _score += points * Difficulty;
            ScoreUpdated?.Invoke(_score);
        }

        public void StartGame(int difficulty)
        {
            //_score += points * Difficulty;
            //ScoreUpdated?.Invoke(_score);
        }

        private void RemoveButtonFromArray(IMusicalButton button)
        {
            int index = Array.IndexOf(_activeButtons, button);
            if (index >= 0)
            {
                var newarr = new IMusicalButton[_activeButtons.Length - 1];
                Array.Copy(_activeButtons, 0, newarr, 0, index);
                Array.Copy(_activeButtons, index + 1, newarr, index, _activeButtons.Length - index - 1);
                _activeButtons = newarr;
            }
        }
    }
}