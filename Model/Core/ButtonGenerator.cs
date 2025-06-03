using System;

namespace Model.Core
{
    public class ButtonGenerator : IButtonGenerator
    {
        private readonly Random _random = new Random();

        public double GetSpeedMultiplier(int score)
        {
            return 1 + (score / 1000.0);
        }

        public IMusicalButton GenerateRandomButton(int score)
        {
            double roll = _random.NextDouble();
            double doubleTapChance = 0.05 + (score / 5000.0);
            double trapChance = 0.03 + (score / 10000.0);

            if (roll < doubleTapChance)
                return new DoubleTapButton();
            if (roll < doubleTapChance + trapChance)
                return new TrapButton();
            if (roll < 0.7)
                return new ShortButton();

            return new LongButton();
        }
    }
}