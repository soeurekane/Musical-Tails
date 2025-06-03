using System;

namespace Model.Core
{
    public class DoubleTapButton : MusicalButtonBase
    {
        private int _tapCount = 0;
        private DateTime _firstTapTime;
        public override bool IsPressed { get; set; }
        public override int ScoreValue => 30;
        public override string ButtonColor => "Purple";
        public override bool ShouldPenalizeOnMiss => true;

        public bool RegisterTap()
        {
            if (IsPressed) return false;

            if (_tapCount == 0)
            {
                _firstTapTime = DateTime.Now;
                _tapCount++;
                PlaySound();
                return false;
            }

            if ((DateTime.Now - _firstTapTime).TotalSeconds < 0.5)
            {
                IsPressed = true;
                PlaySound();
                PlayCompletionSound();
                return true;
            }

            _tapCount = 0;
            return false;
        }

        private void PlayCompletionSound()
        {
            PlayBeep(1000, 200);
        }

        public override void PlaySound()
        {
            PlayBeep(800 - (_tapCount * 200), 100);
        }

        public override string GetButtonType() => "DoubleTap";
    }
}