using System;

namespace Model.Data
{
    public class GameData
    {
        public HighScore[] HighScores { get; set; } = Array.Empty<HighScore>();
        public int MaxScore { get; set; }
    }
}