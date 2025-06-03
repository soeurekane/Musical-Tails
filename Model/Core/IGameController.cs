using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core
{
    public interface IGameController
    {
        void StartGame(int difficulty);
        void EndGame();
        void AddScore(int points);
        int CurrentScore { get; }
        bool IsGameOver { get; }
    }
}
