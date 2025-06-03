using Model.Core;
using static System.Formats.Asn1.AsnWriter;

public abstract class GameBase : IGameController
{
    public abstract int Difficulty { get; set; }
    public abstract int Score { get; }
    public int CurrentScore => Score;  
    public abstract bool IsGameOver { get; }

    public abstract void StartGame(int difficulty);
    public abstract void EndGame();
    public abstract void AddScore(int points);
}
