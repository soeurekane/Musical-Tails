using System;

namespace Model.Core
{
    public abstract class MusicalButtonBase : IMusicalButton
    {
        public abstract bool IsPressed { get; set; }
        public abstract int ScoreValue { get; }
        public abstract string ButtonColor { get; }
        public virtual bool ShouldPenalizeOnMiss => false;

        public abstract void PlaySound();
        public abstract string GetButtonType();
        protected void PlayBeep(int frequency, int duration)
        {
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    Console.Beep(frequency, duration);
                }
                catch { /* Игнорируем ошибки */ }
            }
        }
       
    }
}