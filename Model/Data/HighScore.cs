using System;
using System.Xml.Serialization;

namespace Model.Data
{
    public class HighScore : IComparable<HighScore>
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public int Difficulty { get; set; }
        public DateTime Date { get; set; }

         //перегрузка
        public HighScore()
        {
            Date = DateTime.Now;
        }

        public HighScore(string playerName, int score, int difficulty)
        {
            PlayerName = playerName;
            Score = score;
            Difficulty = difficulty;
            Date = DateTime.Now;
        }
        public int CompareTo(HighScore other)
        {
            if (other == null) return 1;
            return other.Score.CompareTo(this.Score);
        }
        // перегрузка операторов
        public static bool operator >(HighScore a, HighScore b) => a?.Score > b?.Score;
        public static bool operator <(HighScore a, HighScore b) => a?.Score < b?.Score;
        public static bool operator ==(HighScore a, HighScore b) => a?.Score == b?.Score;
        public static bool operator !=(HighScore a, HighScore b) => !(a == b);

        public override bool Equals(object obj) => obj is HighScore other && this == other;
        public override int GetHashCode() => Score.GetHashCode();

        public override string ToString()
        {
            return $"{PlayerName}: {Score} (Уровень: {Difficulty}) - {Date:dd.MM.yyyy}";
        }
    }
}
