using System;
using System.Diagnostics;

namespace Model.Core
{
    public class LongButton : MusicalButtonBase
    {
        private Stopwatch _pressTimer = new Stopwatch();
        private bool _isHolding = false;
        public bool WasAttempted { get; private set; } 
        public override bool IsPressed { get; set; }
        public override int ScoreValue => 20; 
        public override string ButtonColor => _isHolding ? "#00FF00" : "#FF0000"; 
        public override bool ShouldPenalizeOnMiss => true;

        public override string GetButtonType() => "Long";

        public void StartPress()
        {
            if (IsPressed) return;
            WasAttempted = true;
            _isHolding = true;
            _pressTimer.Restart();
            PlaySound();
        }

        public bool EndPress()
        {
            if (IsPressed || !_isHolding) return false;

            _pressTimer.Stop();
            _isHolding = false;

            if (_pressTimer.Elapsed.TotalSeconds >= 0.2)
            {
                IsPressed = true;
                PlayCompletionSound();
                return true;
            }

            PlayFailSound();
            return false;
        }

        private void PlayCompletionSound()
        {
            PlayBeep(800, 300);
        }

        private void PlayFailSound()
        {
            PlayBeep(400, 200);
        }

        public override void PlaySound()
        {
            PlayBeep(600, 100);
        }
    }
}